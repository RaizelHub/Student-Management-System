using System;
using System.Diagnostics;
using System.IO;
using System.Configuration;
using StudentAttendanceSysttem.Utilities;

namespace StudentAttendanceSysttem.Services
{
    /// <summary>
    /// Provides database backup and restore via mysqldump/mysql CLI tools.
    /// Requires mysqldump.exe and mysql.exe to be on PATH or configured in App.config (MySqlBinPath).
    /// </summary>
    public class BackupService
    {
        private static readonly string MysqlBinPath =
            ConfigurationManager.AppSettings["MySqlBinPath"] ?? string.Empty;

        private string Tool(string name) =>
            string.IsNullOrEmpty(MysqlBinPath) ? name
            : Path.Combine(MysqlBinPath, name);

        // ─── Backup ───────────────────────────────────────────────────────────────

        /// <summary>
        /// Runs mysqldump and saves output to <paramref name="outputPath"/>.
        /// Returns (true, outputPath) on success.
        /// </summary>
        public (bool Success, string Message) Backup(string outputPath, string host, string database, string user, string password)
        {
            try
            {
                string args = $"--host={host} --user={user} --password={password} " +
                              $"--single-transaction --routines --triggers {database}";

                var psi = new ProcessStartInfo
                {
                    FileName               = Tool("mysqldump.exe"),
                    Arguments              = args,
                    RedirectStandardOutput = true,
                    RedirectStandardError  = true,
                    UseShellExecute        = false,
                    CreateNoWindow         = true
                };

                using var proc = Process.Start(psi)!;
                string output = proc.StandardOutput.ReadToEnd();
                string errors = proc.StandardError.ReadToEnd();
                proc.WaitForExit();

                if (proc.ExitCode != 0)
                    return (false, $"mysqldump error: {errors}");

                File.WriteAllText(outputPath, output);
                return (true, $"Backup saved to {outputPath}");
            }
            catch (Exception ex)
            {
                return (false, $"Backup failed: {ex.Message}");
            }
        }

        // ─── Restore ──────────────────────────────────────────────────────────────

        /// <summary>
        /// Restores a .sql dump file into the target database.
        /// </summary>
        public (bool Success, string Message) Restore(string dumpPath, string host, string database, string user, string password)
        {
            if (!File.Exists(dumpPath))
                return (false, $"File not found: {dumpPath}");

            try
            {
                string args = $"--host={host} --user={user} --password={password} {database}";

                var psi = new ProcessStartInfo
                {
                    FileName              = Tool("mysql.exe"),
                    Arguments             = args,
                    RedirectStandardInput = true,
                    RedirectStandardError = true,
                    UseShellExecute       = false,
                    CreateNoWindow        = true
                };

                using var proc = Process.Start(psi)!;
                string sql = File.ReadAllText(dumpPath);
                proc.StandardInput.Write(sql);
                proc.StandardInput.Close();

                string errors = proc.StandardError.ReadToEnd();
                proc.WaitForExit();

                if (proc.ExitCode != 0)
                    return (false, $"Restore error: {errors}");

                return (true, "Database restored successfully.");
            }
            catch (Exception ex)
            {
                return (false, $"Restore failed: {ex.Message}");
            }
        }
    }
}

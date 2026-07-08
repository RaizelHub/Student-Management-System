using System;
using MySqlConnector;
using StudentAttendanceSysttem.Database;
using StudentAttendanceSysttem.Utilities;

namespace StudentAttendanceSysttem.Services
{
    /// <summary>
    /// Writes audit log entries to the <c>logs</c> table.
    ///
    /// ASSUMPTION: logs table columns are:
    ///   id, user_id, username, action, description, ip_address, created_at
    /// </summary>
    public class AuditService
    {
        private readonly DatabaseConnection _db;

        public AuditService(DatabaseConnection db) => _db = db;

        // ─── Log Actions ──────────────────────────────────────────────────────────
        public void LogLogin(string username) =>
            WriteLog("LOGIN", $"User '{username}' logged in successfully.");

        public void LogLogout(string username) =>
            WriteLog("LOGOUT", $"User '{username}' logged out.");

        public void LogFailedLogin(string username) =>
            WriteLog("LOGIN_FAILED", $"Failed login attempt for username '{username}'.");

        public void LogAction(string action, string description) =>
            WriteLog(action, description);

        // ─── Core Writer ──────────────────────────────────────────────────────────
        private void WriteLog(string action, string description)
        {
            try
            {
                using var conn = _db.GetConnection();
                conn.Open();
                const string sql = @"
                    INSERT INTO logs (user_id, username, action, description, ip_address, created_at)
                    VALUES (@userId, @username, @action, @description, @ip, NOW())";

                using var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userId",      SessionManager.Instance.UserId);
                cmd.Parameters.AddWithValue("@username",    SessionManager.Instance.Username);
                cmd.Parameters.AddWithValue("@action",      action);
                cmd.Parameters.AddWithValue("@description", description);
                cmd.Parameters.AddWithValue("@ip",          GetLocalIpAddress());
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[AuditService] Failed to write log: {ex.Message}");
                // Audit failures should never crash the application
            }
        }

        private static string GetLocalIpAddress()
        {
            try
            {
                var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
                foreach (var ip in host.AddressList)
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        return ip.ToString();
            }
            catch { }
            return "127.0.0.1";
        }
    }
}

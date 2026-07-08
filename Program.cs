using System;
using System.Windows.Forms;
using StudentAttendanceSysttem.Database;
using StudentAttendanceSysttem.Forms;
using StudentAttendanceSysttem.Utilities;

namespace StudentAttendanceSysttem
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // ── DPI Awareness — prevents blurry/overlapping UI on scaled displays ──
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // ── Global unhandled exception handlers ───────────────────────────────
            Application.ThreadException += (s, e) =>
                NotificationHelper.ShowError(
                    $"An unexpected error occurred:\n\n{e.Exception.Message}",
                    "Unexpected Error");

            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                NotificationHelper.ShowError(
                    $"A fatal error occurred:\n\n{e.ExceptionObject}",
                    "Fatal Error");

            // ── Verify database connectivity ───────────────────────────────────────
            var db = new DatabaseConnection();
            if (!db.TestConnection())
            {
                MessageBox.Show(
                    "Cannot connect to the database.\n\n" +
                    "Please verify your connection string in App.config and ensure MySQL is running.",
                    "Database Connection Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            // ── Launch ────────────────────────────────────────────────────────────
            Application.Run(new LoginForm());
        }
    }
}
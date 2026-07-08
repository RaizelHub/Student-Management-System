using System;
using System.Configuration;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using StudentAttendanceSysttem.Database;
using StudentAttendanceSysttem.Services;
using StudentAttendanceSysttem.Utilities;

namespace StudentAttendanceSysttem.Forms
{
    public partial class SettingsForm : Form
    {
        private readonly BackupService    _backupService;
        private readonly AuditService     _auditService;
        private readonly DatabaseConnection _db;

        // Parse connection string for display
        private readonly string _host, _database, _user, _password;

        public SettingsForm()
        {
            InitializeComponent();
            _db            = new DatabaseConnection();
            _auditService  = new AuditService(_db);
            _backupService = new BackupService();

            var cs = ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ConnectionString ?? "";
            _host     = GetCsPart(cs, "Server") ?? "localhost";
            _database = GetCsPart(cs, "Database") ?? "student_attendance_db";
            _user     = GetCsPart(cs, "Uid") ?? "root";
            _password = GetCsPart(cs, "Pwd") ?? "";
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            if (!SessionManager.Instance.IsAdmin)
            { NotificationHelper.ShowError("Admin only."); this.Close(); return; }

            ThemeManager.ApplyTheme(this);
            ThemeManager.StylePrimaryButton(btnBackup);
            ThemeManager.StyleSecondaryButton(btnRestore);
            ThemeManager.StyleSecondaryButton(btnToggleTheme);

            lblHost.Text     = $"Host: {_host}";
            lblDatabase.Text = $"Database: {_database}";
            lblUser.Text     = $"User: {_user}";
            chkDarkMode.Checked = ThemeManager.IsDarkMode;
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            using var dlg = new SaveFileDialog
            {
                Title       = "Save Backup",
                Filter      = "SQL Files|*.sql",
                FileName    = $"backup_{_database}_{DateTime.Now:yyyyMMdd_HHmmss}.sql",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };
            if (dlg.ShowDialog() != DialogResult.OK) return;

            btnBackup.Enabled = false;
            btnBackup.Text    = "Backing up...";
            try
            {
                var (ok, msg) = _backupService.Backup(dlg.FileName, _host, _database, _user, _password);
                if (ok) { NotificationHelper.ShowSuccess(msg); _auditService.LogAction("BACKUP", $"Backup to: {dlg.FileName}"); }
                else      NotificationHelper.ShowError(msg);
            }
            finally { btnBackup.Enabled = true; btnBackup.Text = "🗄 Backup Database"; }
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            if (!NotificationHelper.Confirm("⚠️ Restore will OVERWRITE the current database. Continue?")) return;
            using var dlg = new OpenFileDialog { Filter = "SQL Files|*.sql", Title = "Select Backup File" };
            if (dlg.ShowDialog() != DialogResult.OK) return;

            var (ok, msg) = _backupService.Restore(dlg.FileName, _host, _database, _user, _password);
            if (ok) { NotificationHelper.ShowSuccess(msg); _auditService.LogAction("RESTORE", $"Restore from: {dlg.FileName}"); }
            else      NotificationHelper.ShowError(msg);
        }

        private void btnToggleTheme_Click(object sender, EventArgs e)
        {
            ThemeManager.ToggleTheme();
            chkDarkMode.Checked = ThemeManager.IsDarkMode;
            ThemeManager.ApplyTheme(this);
        }

        private static string? GetCsPart(string cs, string key)
        {
            foreach (var part in cs.Split(';'))
            {
                var kv = part.Split('=');
                if (kv.Length == 2 && kv[0].Trim().Equals(key, StringComparison.OrdinalIgnoreCase))
                    return kv[1].Trim();
            }
            return null;
        }
    }
}

using System;
using System.Drawing;
using System.Windows.Forms;
using StudentAttendanceSysttem.Database;
using StudentAttendanceSysttem.Repositories;
using StudentAttendanceSysttem.Services;
using StudentAttendanceSysttem.Utilities;

namespace StudentAttendanceSysttem.Forms
{
    /// <summary>
    /// Login form. BCrypt auth, account lockout, session management, remember-me.
    /// </summary>
    public partial class LoginForm : Form
    {
        private readonly UserService  _userService;
        private readonly AuditService _auditService;

        public LoginForm()
        {
            InitializeComponent();
            var db        = new DatabaseConnection();
            _userService  = new UserService(new UserRepository(db));
            _auditService = new AuditService(db);
        }

        // ─── Load ─────────────────────────────────────────────────────────────────
        private void LoginForm_Load(object sender, EventArgs e)
        {
            // Apply theme colors (does NOT touch positions or sizes)
            pnlCard.BackColor     = Color.FromArgb(28, 28, 44);
            lblTitle.ForeColor    = Color.White;
            lblSubtitle.ForeColor = Color.FromArgb(148, 163, 184);
            lnkForgotPassword.LinkColor       = Color.FromArgb(99, 102, 241);
            lnkForgotPassword.ActiveLinkColor = Color.FromArgb(129, 132, 255);
            lnkForgotPassword.VisitedLinkColor = Color.FromArgb(99, 102, 241);
            chkShowPassword.ForeColor     = Color.FromArgb(148, 163, 184);
            chkRememberMe.ForeColor       = Color.FromArgb(148, 163, 184);
            lblBanner.Visible             = false;

            // Hover effect on Sign In button
            btnLogin.MouseEnter += (s, _) =>
                btnLogin.BackColor = Color.FromArgb(79, 70, 229);
            btnLogin.MouseLeave += (s, _) =>
                btnLogin.BackColor = Color.FromArgb(99, 102, 241);

            // Restore remembered username
            if (Properties.Settings.Default.RememberMe)
            {
                txtUsername.Text      = Properties.Settings.Default.SavedUsername;
                chkRememberMe.Checked = true;
            }

            txtUsername.Focus();
        }

        // ─── Login ────────────────────────────────────────────────────────────────
        private void btnLogin_Click(object sender, EventArgs e) => AttemptLogin();

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) AttemptLogin();
        }

        private void AttemptLogin()
        {
            var username = txtUsername.Text.Trim();
            var password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ShowBanner("Please enter your username and password.", NotificationType.Warning);
                return;
            }

            btnLogin.Enabled = false;
            btnLogin.Text    = "Authenticating...";

            try
            {
                var result = _userService.Authenticate(username, password);

                if (result.IsSuccess)
                {
                    _auditService.LogLogin(username);

                    Properties.Settings.Default.RememberMe    = chkRememberMe.Checked;
                    Properties.Settings.Default.SavedUsername = chkRememberMe.Checked ? username : string.Empty;
                    Properties.Settings.Default.Save();

                    OpenDashboard();
                }
                else
                {
                    _auditService.LogFailedLogin(username);
                    ShowBanner(result.Message, NotificationType.Error);
                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
            catch (Exception ex)
            {
                ShowBanner($"Connection error: {ex.Message}", NotificationType.Error);
            }
            finally
            {
                btnLogin.Enabled = true;
                btnLogin.Text    = "Sign In";
            }
        }

        private void OpenDashboard()
        {
            var dashboard = new DashboardForm();
            dashboard.Show();
            this.Hide();
            dashboard.FormClosed += (s, e) =>
            {
                _userService.Logout();
                this.Show();
                txtPassword.Clear();
                txtUsername.Focus();
            };
        }

        // ─── Show/Hide Password ───────────────────────────────────────────────────
        private void chkShowPassword_CheckedChanged(object sender, EventArgs e) =>
            txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;

        // ─── Forgot Password ──────────────────────────────────────────────────────
        private void lnkForgotPassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) =>
            NotificationHelper.ShowInfo(
                "Please contact your system administrator to reset your password.",
                "Forgot Password");

        // ─── Banner ───────────────────────────────────────────────────────────────
        private void ShowBanner(string message, NotificationType type) =>
            NotificationHelper.ShowBanner(lblBanner, message, type);
    }
}

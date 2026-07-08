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
    /// Main dashboard form with sidebar navigation, stat cards, and recent attendance grid.
    /// </summary>
    public partial class DashboardForm : Form
    {
        private readonly DashboardService _dashService;
        private readonly AuditService     _auditService;
        private readonly DatabaseConnection _db;
        private Form?   _activeChildForm;

        public DashboardForm()
        {
            InitializeComponent();
            _db           = new DatabaseConnection();
            _dashService  = new DashboardService(new DashboardRepository(_db));
            _auditService = new AuditService(_db);
        }

        // ─── Load ─────────────────────────────────────────────────────────────────
        private void DashboardForm_Load(object sender, EventArgs e)
        {
            ThemeManager.ApplyTheme(this);
            StyleSidebar();
            StyleCards();
            ThemeManager.StyleDataGridView(dgvRecent);

            lblUserName.Text = $"👤  {SessionManager.Instance.FullName}";
            lblRole.Text     = SessionManager.Instance.Role;
            lblDateTime.Text = DateTime.Now.ToString("dddd, MMMM dd yyyy");

            // Session expiry timer — checks every minute
            var sessionTimer = new System.Windows.Forms.Timer { Interval = 60_000 };
            sessionTimer.Tick += (s, _) =>
            {
                if (SessionManager.Instance.IsSessionExpired())
                {
                    sessionTimer.Stop();
                    NotificationHelper.ShowWarning("Your session has expired. Please log in again.", "Session Expired");
                    this.Close();
                }
            };
            sessionTimer.Start();

            // Clock timer
            var clockTimer = new System.Windows.Forms.Timer { Interval = 1000 };
            clockTimer.Tick += (s, _) =>
                lblClock.Text = DateTime.Now.ToString("hh:mm:ss tt");
            clockTimer.Start();

            LoadDashboard();

            // Hide admin-only nav buttons for staff
            btnUsers.Visible   = SessionManager.Instance.IsAdmin;
            btnSettings.Visible = SessionManager.Instance.IsAdmin;
        }

        // ─── Dashboard Data ───────────────────────────────────────────────────────
        private void LoadDashboard()
        {
            try
            {
                var stats = _dashService.GetTodayStats();
                lblTotalStudents.Text   = stats.TotalStudents.ToString();
                lblPresentToday.Text    = stats.PresentToday.ToString();
                lblAbsentToday.Text     = stats.AbsentToday.ToString();
                lblLateToday.Text       = stats.LateToday.ToString();
                lblAttendancePct.Text   = $"{stats.AttendancePercentage}%";

                var dt = _dashService.GetRecentAttendance(50);
                dgvRecent.DataSource = dt;
                FormatGrid();
            }
            catch (Exception ex)
            {
                NotificationHelper.ShowError($"Failed to load dashboard: {ex.Message}");
            }
        }

        private void FormatGrid()
        {
            if (dgvRecent.Columns.Count == 0) return;
            if (dgvRecent.Columns.Contains("id"))
                dgvRecent.Columns["id"]!.Visible = false;

            var colHeaders = new[]
            {
                ("student_name",   "Student Name"),
                ("student_number", "Student No."),
                ("course",         "Course"),
                ("section",        "Section"),
                ("date",           "Date"),
                ("time_in",        "Time In"),
                ("time_out",       "Time Out"),
                ("status",         "Status")
            };

            foreach (var (col, header) in colHeaders)
                if (dgvRecent.Columns.Contains(col))
                    dgvRecent.Columns[col]!.HeaderText = header;

            // Status color coding via CellFormatting
            dgvRecent.CellFormatting += (s, e) =>
            {
                if (e.RowIndex < 0 || dgvRecent.Columns[e.ColumnIndex].Name != "status") return;
                e.CellStyle!.ForeColor = e.Value?.ToString() switch
                {
                    "Present" => ThemeManager.Success,
                    "Absent"  => ThemeManager.Danger,
                    "Late"    => ThemeManager.Warning,
                    _         => ThemeManager.TextSecondary
                };
                e.CellStyle.Font = ThemeManager.FontBold;
            };
        }

        // ─── Refresh Button ───────────────────────────────────────────────────────
        private void btnRefresh_Click(object sender, EventArgs e) => LoadDashboard();

        // ─── Sidebar Navigation ───────────────────────────────────────────────────
        private void btnStudents_Click(object sender, EventArgs e)   => OpenForm(new StudentForm());
        private void btnCourses_Click(object sender, EventArgs e)     => OpenForm(new CourseForm());
        private void btnSections_Click(object sender, EventArgs e)    => OpenForm(new SectionForm());
        private void btnAttendance_Click(object sender, EventArgs e)  => OpenForm(new AttendanceForm());
        private void btnReports_Click(object sender, EventArgs e)     => OpenForm(new ReportsForm());
        private void btnUsers_Click(object sender, EventArgs e)       => OpenForm(new UserManagementForm());
        private void btnAuditLogs_Click(object sender, EventArgs e)   => OpenForm(new AuditLogsForm());
        private void btnSettings_Click(object sender, EventArgs e)    => OpenForm(new SettingsForm());

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            _activeChildForm?.Close();
            _activeChildForm = null;
            pnlContent.Visible = false;
            pnlDashboard.Visible = true;
            LoadDashboard();
        }

        private void OpenForm(Form form)
        {
            _activeChildForm?.Close();
            _activeChildForm = null;

            pnlDashboard.Visible = false;
            pnlContent.Visible   = true;
            pnlContent.Controls.Clear();

            form.TopLevel    = false;
            form.Dock        = DockStyle.Fill;
            form.FormBorderStyle = FormBorderStyle.None;
            pnlContent.Controls.Add(form);
            form.Show();
            _activeChildForm = form;
        }

        // ─── Logout ───────────────────────────────────────────────────────────────
        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (!NotificationHelper.Confirm("Are you sure you want to log out?")) return;
            _auditService.LogLogout(SessionManager.Instance.Username);
            SessionManager.Instance.Logout();
            this.Close();
        }

        // ─── Theme Toggle ─────────────────────────────────────────────────────────
        private void btnTheme_Click(object sender, EventArgs e)
        {
            ThemeManager.ToggleTheme();
            ThemeManager.ApplyTheme(this);
            StyleSidebar();
            StyleCards();
        }

        // ─── Styling Helpers ──────────────────────────────────────────────────────
        private void StyleSidebar()
        {
            pnlSidebar.BackColor = ThemeManager.Sidebar;
            foreach (Control c in pnlSidebar.Controls)
            {
                if (c is Button btn)
                {
                    btn.FlatStyle  = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 0;
                    btn.ForeColor  = Color.FromArgb(148, 163, 184);
                    btn.BackColor  = Color.Transparent;
                    btn.Font       = ThemeManager.FontSidebar;
                    btn.TextAlign  = ContentAlignment.MiddleLeft;
                    btn.Padding    = new Padding(16, 0, 0, 0);
                    btn.Cursor     = Cursors.Hand;
                    btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(40, 40, 62);
                }
            }
        }

        private void StyleCards()
        {
            StyleCard(pnlCardTotal,   lblTotalStudents,   lblCardTotal,   ThemeManager.Primary);
            StyleCard(pnlCardPresent, lblPresentToday,    lblCardPresent, ThemeManager.Success);
            StyleCard(pnlCardAbsent,  lblAbsentToday,     lblCardAbsent,  ThemeManager.Danger);
            StyleCard(pnlCardLate,    lblLateToday,        lblCardLate,   ThemeManager.Warning);
            StyleCard(pnlCardPct,     lblAttendancePct,   lblCardPct,    ThemeManager.Info);
        }

        private static void StyleCard(Panel panel, Label valueLabel, Label titleLabel, Color accent)
        {
            panel.BackColor    = ThemeManager.CardBackground;
            valueLabel.Font    = ThemeManager.FontCard;
            valueLabel.ForeColor = accent;
            titleLabel.Font    = ThemeManager.FontCardSub;
            titleLabel.ForeColor = ThemeManager.TextSecondary;

            // Left accent border using Paint
            panel.Paint -= (s, e) => { }; // remove previous handlers if any
            panel.Paint += (s, e) =>
            {
                using var pen = new Pen(accent, 4);
                e.Graphics.DrawLine(pen, 0, 8, 0, panel.Height - 8);
            };
        }
    }
}

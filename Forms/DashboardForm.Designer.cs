namespace StudentAttendanceSysttem.Forms
{
    partial class DashboardForm
    {
        private System.ComponentModel.IContainer components = null;

        // ── Layout panels ─────────────────────────────────────────────────────────
        private System.Windows.Forms.Panel pnlSidebar;
        private System.Windows.Forms.Panel pnlTopBar;
        private System.Windows.Forms.Panel pnlDashboard;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Panel pnlCards;

        // ── Sidebar buttons ───────────────────────────────────────────────────────
        private System.Windows.Forms.Button btnDashboard;
        private System.Windows.Forms.Button btnStudents;
        private System.Windows.Forms.Button btnCourses;
        private System.Windows.Forms.Button btnSections;
        private System.Windows.Forms.Button btnAttendance;
        private System.Windows.Forms.Button btnReports;
        private System.Windows.Forms.Button btnUsers;
        private System.Windows.Forms.Button btnAuditLogs;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnTheme;

        // ── Top bar ───────────────────────────────────────────────────────────────
        private System.Windows.Forms.Label lblDateTime;
        private System.Windows.Forms.Label lblClock;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Label lblRole;
        private System.Windows.Forms.Button btnRefresh;

        // ── Stat cards ────────────────────────────────────────────────────────────
        private System.Windows.Forms.Panel pnlCardTotal;
        private System.Windows.Forms.Panel pnlCardPresent;
        private System.Windows.Forms.Panel pnlCardAbsent;
        private System.Windows.Forms.Panel pnlCardLate;
        private System.Windows.Forms.Panel pnlCardPct;

        private System.Windows.Forms.Label lblTotalStudents;
        private System.Windows.Forms.Label lblPresentToday;
        private System.Windows.Forms.Label lblAbsentToday;
        private System.Windows.Forms.Label lblLateToday;
        private System.Windows.Forms.Label lblAttendancePct;

        private System.Windows.Forms.Label lblCardTotal;
        private System.Windows.Forms.Label lblCardPresent;
        private System.Windows.Forms.Label lblCardAbsent;
        private System.Windows.Forms.Label lblCardLate;
        private System.Windows.Forms.Label lblCardPct;

        // ── Grid ──────────────────────────────────────────────────────────────────
        private System.Windows.Forms.DataGridView dgvRecent;
        private System.Windows.Forms.Label        lblRecentTitle;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            const int SIDEBAR_W = 220;
            const int TOPBAR_H  = 64;
            const int CARD_H    = 110;

            // ── Form ──────────────────────────────────────────────────────────────
            this.Text            = "Student Attendance System — Dashboard";
            this.Size            = new System.Drawing.Size(1280, 800);
            this.StartPosition   = FormStartPosition.CenterScreen;
            this.MinimumSize     = new System.Drawing.Size(1100, 700);
            this.BackColor       = System.Drawing.Color.FromArgb(18, 18, 30);
            this.Load           += DashboardForm_Load;

            // ── Sidebar ───────────────────────────────────────────────────────────
            pnlSidebar = new System.Windows.Forms.Panel
            {
                Dock      = DockStyle.Left,
                Width     = SIDEBAR_W,
                BackColor = System.Drawing.Color.FromArgb(22, 22, 38)
            };

            var lblSidebarLogo = new System.Windows.Forms.Label
            {
                Text      = "🎓 SAMS",
                Font      = new System.Drawing.Font("Segoe UI", 14f, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.FromArgb(99, 102, 241),
                AutoSize  = true,
                Location  = new System.Drawing.Point(16, 20)
            };

            btnDashboard  = MakeSidebarButton("🏠  Dashboard",   65);
            btnStudents   = MakeSidebarButton("👨‍🎓  Students",      115);
            btnCourses    = MakeSidebarButton("📚  Courses",      165);
            btnSections   = MakeSidebarButton("🏫  Sections",     215);
            btnAttendance = MakeSidebarButton("✅  Attendance",   265);
            btnReports    = MakeSidebarButton("📊  Reports",      315);
            btnUsers      = MakeSidebarButton("👥  Users",         365);
            btnAuditLogs  = MakeSidebarButton("📋  Audit Logs",   415);
            btnSettings   = MakeSidebarButton("⚙️  Settings",     465);
            btnTheme      = MakeSidebarButton("🌙  Toggle Theme", 700);
            btnLogout     = MakeSidebarButton("🚪  Logout",       740);

            btnDashboard.Click  += btnDashboard_Click;
            btnStudents.Click   += btnStudents_Click;
            btnCourses.Click    += btnCourses_Click;
            btnSections.Click   += btnSections_Click;
            btnAttendance.Click += btnAttendance_Click;
            btnReports.Click    += btnReports_Click;
            btnUsers.Click      += btnUsers_Click;
            btnAuditLogs.Click  += btnAuditLogs_Click;
            btnSettings.Click   += btnSettings_Click;
            btnTheme.Click      += btnTheme_Click;
            btnLogout.Click     += btnLogout_Click;

            pnlSidebar.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                lblSidebarLogo,
                btnDashboard, btnStudents, btnCourses, btnSections,
                btnAttendance, btnReports, btnUsers, btnAuditLogs, btnSettings,
                btnTheme, btnLogout
            });

            // ── Top Bar ───────────────────────────────────────────────────────────
            pnlTopBar = new System.Windows.Forms.Panel
            {
                Dock      = DockStyle.Top,
                Height    = TOPBAR_H,
                BackColor = System.Drawing.Color.FromArgb(28, 28, 44),
                Padding   = new System.Windows.Forms.Padding(16, 0, 16, 0)
            };
            pnlTopBar.BringToFront();

            lblDateTime = new System.Windows.Forms.Label
            {
                AutoSize  = true,
                Location  = new System.Drawing.Point(16, 12),
                Font      = new System.Drawing.Font("Segoe UI", 10f),
                ForeColor = System.Drawing.Color.FromArgb(148, 163, 184)
            };

            lblClock = new System.Windows.Forms.Label
            {
                AutoSize  = true,
                Location  = new System.Drawing.Point(16, 32),
                Font      = new System.Drawing.Font("Segoe UI", 11f, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.White
            };

            lblUserName = new System.Windows.Forms.Label
            {
                AutoSize  = true,
                Location  = new System.Drawing.Point(700, 12),
                Font      = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.White
            };

            lblRole = new System.Windows.Forms.Label
            {
                AutoSize  = true,
                Location  = new System.Drawing.Point(720, 34),
                Font      = new System.Drawing.Font("Segoe UI", 8.5f),
                ForeColor = System.Drawing.Color.FromArgb(99, 102, 241)
            };

            btnRefresh = new System.Windows.Forms.Button
            {
                Text      = "🔄 Refresh",
                Location  = new System.Drawing.Point(900, 16),
                Size      = new System.Drawing.Size(110, 32),
                FlatStyle = FlatStyle.Flat,
                Cursor    = System.Windows.Forms.Cursors.Hand
            };
            btnRefresh.Click += btnRefresh_Click;

            pnlTopBar.Controls.AddRange(new System.Windows.Forms.Control[]
                { lblDateTime, lblClock, lblUserName, lblRole, btnRefresh });

            // ── Dashboard content ──────────────────────────────────────────────────
            pnlDashboard = new System.Windows.Forms.Panel
            {
                Dock      = DockStyle.Fill,
                BackColor = System.Drawing.Color.FromArgb(18, 18, 30),
                Padding   = new System.Windows.Forms.Padding(20)
            };

            // Cards row
            pnlCards = new System.Windows.Forms.Panel
            {
                Dock      = DockStyle.Top,
                Height    = CARD_H + 20,
                BackColor = System.Drawing.Color.Transparent
            };

            pnlCardTotal   = MakeCard("Total Students",       out lblTotalStudents,   out lblCardTotal,   0);
            pnlCardPresent = MakeCard("Present Today",         out lblPresentToday,    out lblCardPresent, 1);
            pnlCardAbsent  = MakeCard("Absent Today",          out lblAbsentToday,     out lblCardAbsent,  2);
            pnlCardLate    = MakeCard("Late Today",            out lblLateToday,       out lblCardLate,    3);
            pnlCardPct     = MakeCard("Attendance %",         out lblAttendancePct,   out lblCardPct,     4);

            pnlCards.Controls.AddRange(new System.Windows.Forms.Control[]
                { pnlCardTotal, pnlCardPresent, pnlCardAbsent, pnlCardLate, pnlCardPct });

            // Recent grid
            lblRecentTitle = new System.Windows.Forms.Label
            {
                Text      = "Recent Attendance Records",
                Font      = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.White,
                AutoSize  = true,
                Location  = new System.Drawing.Point(0, CARD_H + 30)
            };

            dgvRecent = new System.Windows.Forms.DataGridView
            {
                Location = new System.Drawing.Point(0, CARD_H + 60),
                Size     = new System.Drawing.Size(1020, 440),
                Anchor   = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
            };

            pnlDashboard.Controls.AddRange(new System.Windows.Forms.Control[]
                { pnlCards, lblRecentTitle, dgvRecent });

            // ── Inner pnlContent (for embedded child forms) ───────────────────────
            pnlContent = new System.Windows.Forms.Panel
            {
                Dock    = DockStyle.Fill,
                Visible = false
            };

            this.Controls.AddRange(new System.Windows.Forms.Control[]
                { pnlContent, pnlDashboard, pnlTopBar, pnlSidebar });
        }

        // ─── Sidebar Button Factory ───────────────────────────────────────────────
        private static System.Windows.Forms.Button MakeSidebarButton(string text, int top) =>
            new System.Windows.Forms.Button
            {
                Text      = text,
                Location  = new System.Drawing.Point(0, top),
                Size      = new System.Drawing.Size(220, 44),
                FlatStyle = FlatStyle.Flat,
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                Padding   = new System.Windows.Forms.Padding(16, 0, 0, 0),
                Cursor    = System.Windows.Forms.Cursors.Hand
            };

        // ─── Card Factory ─────────────────────────────────────────────────────────
        private static System.Windows.Forms.Panel MakeCard(
            string title,
            out System.Windows.Forms.Label valueLabel,
            out System.Windows.Forms.Label titleLabel,
            int index)
        {
            const int CARD_W = 190;
            const int GAP    = 10;

            var panel = new System.Windows.Forms.Panel
            {
                Location = new System.Drawing.Point(index * (CARD_W + GAP), 10),
                Size     = new System.Drawing.Size(CARD_W, 100),
                Padding  = new System.Windows.Forms.Padding(14)
            };

            var val = new System.Windows.Forms.Label
            {
                Text     = "—",
                AutoSize = true,
                Location = new System.Drawing.Point(14, 16)
            };

            var lbl = new System.Windows.Forms.Label
            {
                Text     = title,
                AutoSize = true,
                Location = new System.Drawing.Point(14, 66)
            };

            panel.Controls.AddRange(new System.Windows.Forms.Control[] { val, lbl });
            valueLabel = val;
            titleLabel = lbl;
            return panel;
        }
    }
}

namespace StudentAttendanceSysttem.Forms
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;

        // Controls
        private System.Windows.Forms.Panel      pnlBackground;
        private System.Windows.Forms.Panel      pnlCard;
        private System.Windows.Forms.Label      lblTitle;
        private System.Windows.Forms.Label      lblSubtitle;
        private System.Windows.Forms.Label      lblUsernameHdr;
        private System.Windows.Forms.Label      lblPasswordHdr;
        private System.Windows.Forms.TextBox    txtUsername;
        private System.Windows.Forms.TextBox    txtPassword;
        private System.Windows.Forms.CheckBox   chkShowPassword;
        private System.Windows.Forms.CheckBox   chkRememberMe;
        private System.Windows.Forms.Button     btnLogin;
        private System.Windows.Forms.LinkLabel  lnkForgotPassword;
        private System.Windows.Forms.Label      lblBanner;
        private System.Windows.Forms.Label      lblVersion;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            // ── Form ──────────────────────────────────────────────────────────────
            this.Text            = "Student Attendance System — Login";
            this.Size            = new System.Drawing.Size(940, 620);
            this.StartPosition   = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox     = false;
            this.MinimizeBox     = true;
            this.BackColor       = System.Drawing.Color.FromArgb(18, 18, 30);
            this.Load           += LoginForm_Load;

            // ── Left branding panel ────────────────────────────────────────────────
            pnlBackground = new System.Windows.Forms.Panel
            {
                Location  = new System.Drawing.Point(0, 0),
                Size      = new System.Drawing.Size(390, 620),
                BackColor = System.Drawing.Color.FromArgb(22, 22, 38)
            };

            // Graduation cap icon
            var lblIcon = new System.Windows.Forms.Label
            {
                Text      = "🎓",
                Font      = new System.Drawing.Font("Segoe UI Emoji", 52f),
                ForeColor = System.Drawing.Color.FromArgb(99, 102, 241),
                AutoSize  = true,
                Location  = new System.Drawing.Point(145, 160)
            };

            var lblBrandTitle = new System.Windows.Forms.Label
            {
                Text      = "Student Attendance",
                Font      = new System.Drawing.Font("Segoe UI", 17f, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.White,
                AutoSize  = true,
                Location  = new System.Drawing.Point(50, 280)
            };

            var lblBrandSub = new System.Windows.Forms.Label
            {
                Text      = "Management System",
                Font      = new System.Drawing.Font("Segoe UI", 13f),
                ForeColor = System.Drawing.Color.FromArgb(148, 163, 184),
                AutoSize  = true,
                Location  = new System.Drawing.Point(95, 312)
            };

            var lblBrandDesc = new System.Windows.Forms.Label
            {
                Text      = "Track attendance, manage students,\nand generate insightful reports.",
                Font      = new System.Drawing.Font("Segoe UI", 9f),
                ForeColor = System.Drawing.Color.FromArgb(100, 116, 139),
                Size      = new System.Drawing.Size(290, 50),
                Location  = new System.Drawing.Point(50, 368),
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            };

            pnlBackground.Controls.AddRange(new System.Windows.Forms.Control[]
                { lblIcon, lblBrandTitle, lblBrandSub, lblBrandDesc });

            // ── Login Card ─────────────────────────────────────────────────────────
            // Sits to the RIGHT of the left panel, centred vertically
            pnlCard = new System.Windows.Forms.Panel
            {
                Location  = new System.Drawing.Point(420, 60),
                Size      = new System.Drawing.Size(480, 500),
                BackColor = System.Drawing.Color.FromArgb(28, 28, 44)
            };

            // Row 1 — Title  (y=30)
            lblTitle = new System.Windows.Forms.Label
            {
                Text      = "Welcome Back",
                Font      = new System.Drawing.Font("Segoe UI", 22f, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.White,
                AutoSize  = true,
                Location  = new System.Drawing.Point(40, 30)
            };

            // Row 2 — Subtitle  (y=82)
            lblSubtitle = new System.Windows.Forms.Label
            {
                Text      = "Sign in to your account",
                Font      = new System.Drawing.Font("Segoe UI", 11f),
                ForeColor = System.Drawing.Color.FromArgb(148, 163, 184),
                AutoSize  = true,
                Location  = new System.Drawing.Point(40, 82)
            };

            // Row 3 — Banner  (y=115)
            lblBanner = new System.Windows.Forms.Label
            {
                Location  = new System.Drawing.Point(40, 115),
                Size      = new System.Drawing.Size(400, 36),
                Visible   = false,
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                Font      = new System.Drawing.Font("Segoe UI", 9.5f),
                ForeColor = System.Drawing.Color.White
            };

            // Row 4 — Username label  (y=163)
            lblUsernameHdr = new System.Windows.Forms.Label
            {
                Text      = "Username",
                Font      = new System.Drawing.Font("Segoe UI", 9f),
                ForeColor = System.Drawing.Color.FromArgb(148, 163, 184),
                AutoSize  = true,
                Location  = new System.Drawing.Point(40, 163)
            };

            // Row 5 — Username textbox  (y=186)
            txtUsername = new System.Windows.Forms.TextBox
            {
                Location        = new System.Drawing.Point(40, 186),
                Size            = new System.Drawing.Size(400, 36),
                MaxLength       = 50,
                PlaceholderText = "Enter your username",
                Font            = new System.Drawing.Font("Segoe UI", 10.5f),
                BackColor       = System.Drawing.Color.FromArgb(35, 35, 55),
                ForeColor       = System.Drawing.Color.White,
                BorderStyle     = BorderStyle.FixedSingle
            };

            // Row 6 — Password label  (y=240)
            lblPasswordHdr = new System.Windows.Forms.Label
            {
                Text      = "Password",
                Font      = new System.Drawing.Font("Segoe UI", 9f),
                ForeColor = System.Drawing.Color.FromArgb(148, 163, 184),
                AutoSize  = true,
                Location  = new System.Drawing.Point(40, 240)
            };

            // Row 7 — Password textbox  (y=263)
            txtPassword = new System.Windows.Forms.TextBox
            {
                Location              = new System.Drawing.Point(40, 263),
                Size                  = new System.Drawing.Size(400, 36),
                UseSystemPasswordChar = true,
                MaxLength             = 100,
                PlaceholderText       = "Enter your password",
                Font                  = new System.Drawing.Font("Segoe UI", 10.5f),
                BackColor             = System.Drawing.Color.FromArgb(35, 35, 55),
                ForeColor             = System.Drawing.Color.White,
                BorderStyle           = BorderStyle.FixedSingle
            };
            txtPassword.KeyDown += txtPassword_KeyDown;

            // Row 8 — Checkboxes  (y=314)
            chkShowPassword = new System.Windows.Forms.CheckBox
            {
                Text      = "Show Password",
                Font      = new System.Drawing.Font("Segoe UI", 9f),
                ForeColor = System.Drawing.Color.FromArgb(148, 163, 184),
                AutoSize  = true,
                Location  = new System.Drawing.Point(40, 314),
                Cursor    = System.Windows.Forms.Cursors.Hand
            };
            chkShowPassword.CheckedChanged += chkShowPassword_CheckedChanged;

            chkRememberMe = new System.Windows.Forms.CheckBox
            {
                Text      = "Remember Me",
                Font      = new System.Drawing.Font("Segoe UI", 9f),
                ForeColor = System.Drawing.Color.FromArgb(148, 163, 184),
                AutoSize  = true,
                Location  = new System.Drawing.Point(240, 314),
                Cursor    = System.Windows.Forms.Cursors.Hand
            };

            // Row 9 — Sign In button  (y=352)
            btnLogin = new System.Windows.Forms.Button
            {
                Text      = "Sign In",
                Location  = new System.Drawing.Point(40, 352),
                Size      = new System.Drawing.Size(400, 46),
                Font      = new System.Drawing.Font("Segoe UI", 11f, System.Drawing.FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                BackColor = System.Drawing.Color.FromArgb(99, 102, 241),
                ForeColor = System.Drawing.Color.White,
                Cursor    = System.Windows.Forms.Cursors.Hand
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += btnLogin_Click;

            // Row 10 — Forgot password  (y=415)
            lnkForgotPassword = new System.Windows.Forms.LinkLabel
            {
                Text      = "Forgot your password?",
                Font      = new System.Drawing.Font("Segoe UI", 9f),
                AutoSize  = true,
                Location  = new System.Drawing.Point(165, 415),
                Cursor    = System.Windows.Forms.Cursors.Hand
            };
            lnkForgotPassword.LinkClicked += lnkForgotPassword_LinkClicked;

            pnlCard.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                lblTitle, lblSubtitle, lblBanner,
                lblUsernameHdr, txtUsername,
                lblPasswordHdr, txtPassword,
                chkShowPassword, chkRememberMe,
                btnLogin, lnkForgotPassword
            });

            // Version label (bottom of form)
            lblVersion = new System.Windows.Forms.Label
            {
                Text      = "v1.0.0 — Student Attendance Management System",
                AutoSize  = true,
                Location  = new System.Drawing.Point(420, 578),
                ForeColor = System.Drawing.Color.FromArgb(55, 55, 80),
                Font      = new System.Drawing.Font("Segoe UI", 7.5f)
            };

            this.Controls.AddRange(new System.Windows.Forms.Control[]
                { pnlBackground, pnlCard, lblVersion });
        }
    }
}

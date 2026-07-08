namespace StudentAttendanceSysttem.Forms
{
    partial class SettingsForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle, lblHost, lblDatabase, lblUser, lblDbSection, lblThemeSection, lblBkSection;
        private System.Windows.Forms.CheckBox chkDarkMode;
        private System.Windows.Forms.Button btnBackup, btnRestore, btnToggleTheme;
        protected override void Dispose(bool d) { if (d && components != null) components.Dispose(); base.Dispose(d); }
        private void InitializeComponent()
        {
            this.Text = "Settings"; this.BackColor = System.Drawing.Color.FromArgb(18,18,30); this.Dock = DockStyle.Fill; this.Load += SettingsForm_Load;
            lblTitle = new System.Windows.Forms.Label { Text="⚙️  Settings", Font=new System.Drawing.Font("Segoe UI",16f,System.Drawing.FontStyle.Bold), ForeColor=System.Drawing.Color.White, AutoSize=true, Location=new System.Drawing.Point(20,16) };

            // DB Info
            lblDbSection = Sec("📡 Database Connection", 20, 60);
            lblHost      = Info("Host: —",     20, 90);
            lblDatabase  = Info("Database: —", 20, 112);
            lblUser      = Info("User: —",     20, 134);

            // Theme
            lblThemeSection = Sec("🎨 Appearance", 20, 180);
            chkDarkMode = new System.Windows.Forms.CheckBox { Text="Dark Mode", Location=new System.Drawing.Point(20,206), AutoSize=true, ForeColor=System.Drawing.Color.FromArgb(148,163,184) };
            btnToggleTheme = new System.Windows.Forms.Button { Text="🌙 Toggle Theme", Location=new System.Drawing.Point(20,232), Size=new System.Drawing.Size(160,34) };
            btnToggleTheme.Click += btnToggleTheme_Click;

            // Backup
            lblBkSection = Sec("🗄 Backup & Restore", 20, 290);
            btnBackup  = new System.Windows.Forms.Button { Text="🗄 Backup Database",  Location=new System.Drawing.Point(20,320), Size=new System.Drawing.Size(200,40), Font=new System.Drawing.Font("Segoe UI",10f) };
            btnRestore = new System.Windows.Forms.Button { Text="🔄 Restore Database", Location=new System.Drawing.Point(240,320), Size=new System.Drawing.Size(200,40), Font=new System.Drawing.Font("Segoe UI",10f) };
            btnBackup.Click  += btnBackup_Click;
            btnRestore.Click += btnRestore_Click;

            var warn = new System.Windows.Forms.Label { Text="⚠️ Restore will overwrite the current database. Create a backup first.", AutoSize=true, Location=new System.Drawing.Point(20,372), ForeColor=System.Drawing.Color.FromArgb(234,179,8), Font=new System.Drawing.Font("Segoe UI",9f) };

            this.Controls.AddRange(new System.Windows.Forms.Control[] { lblTitle, lblDbSection, lblHost, lblDatabase, lblUser, lblThemeSection, chkDarkMode, btnToggleTheme, lblBkSection, btnBackup, btnRestore, warn });
        }
        private static System.Windows.Forms.Label Sec(string t,int x,int y) => new System.Windows.Forms.Label{Text=t,AutoSize=true,Location=new System.Drawing.Point(x,y),Font=new System.Drawing.Font("Segoe UI",11f,System.Drawing.FontStyle.Bold),ForeColor=System.Drawing.Color.FromArgb(99,102,241)};
        private static System.Windows.Forms.Label Info(string t,int x,int y) => new System.Windows.Forms.Label{Text=t,AutoSize=true,Location=new System.Drawing.Point(x,y),ForeColor=System.Drawing.Color.FromArgb(148,163,184)};
    }
}

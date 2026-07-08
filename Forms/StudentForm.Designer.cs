namespace StudentAttendanceSysttem.Forms
{
    partial class StudentForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel          pnlToolbar;
        private System.Windows.Forms.TextBox        txtSearch;
        private System.Windows.Forms.Label          lblSearch;
        private System.Windows.Forms.Button         btnAdd;
        private System.Windows.Forms.Button         btnEdit;
        private System.Windows.Forms.Button         btnDelete;
        private System.Windows.Forms.Button         btnRefresh;
        private System.Windows.Forms.DataGridView   dgvStudents;
        private System.Windows.Forms.Label          lblTitle;
        private System.Windows.Forms.Label          lblCount;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.Text      = "Student Management";
            this.BackColor = System.Drawing.Color.FromArgb(18, 18, 30);
            this.Dock      = DockStyle.Fill;
            this.Load     += StudentForm_Load;

            // Title
            lblTitle = new System.Windows.Forms.Label
            {
                Text      = "👨‍🎓  Student Management",
                Font      = new System.Drawing.Font("Segoe UI", 16f, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.White,
                AutoSize  = true,
                Location  = new System.Drawing.Point(20, 20)
            };

            // Toolbar
            pnlToolbar = new System.Windows.Forms.Panel
            {
                Location  = new System.Drawing.Point(20, 60),
                Size      = new System.Drawing.Size(980, 46),
                BackColor = System.Drawing.Color.Transparent
            };

            lblSearch = new System.Windows.Forms.Label
            {
                Text      = "🔍",
                AutoSize  = true,
                Location  = new System.Drawing.Point(0, 12),
                ForeColor = System.Drawing.Color.White
            };

            txtSearch = new System.Windows.Forms.TextBox
            {
                Location        = new System.Drawing.Point(24, 8),
                Size            = new System.Drawing.Size(280, 30),
                PlaceholderText = "Search by name, student no, email..."
            };
            txtSearch.TextChanged += txtSearch_TextChanged;

            btnAdd     = new System.Windows.Forms.Button { Text = "➕ Add Student",   Location = new System.Drawing.Point(320, 8),  Size = new System.Drawing.Size(130, 32) };
            btnEdit    = new System.Windows.Forms.Button { Text = "✏️ Edit",          Location = new System.Drawing.Point(460, 8),  Size = new System.Drawing.Size(100, 32) };
            btnDelete  = new System.Windows.Forms.Button { Text = "🗑️ Delete",       Location = new System.Drawing.Point(570, 8),  Size = new System.Drawing.Size(100, 32) };
            btnRefresh = new System.Windows.Forms.Button { Text = "🔄 Refresh",       Location = new System.Drawing.Point(680, 8),  Size = new System.Drawing.Size(100, 32) };

            btnAdd.Click     += btnAdd_Click;
            btnEdit.Click    += btnEdit_Click;
            btnDelete.Click  += btnDelete_Click;
            btnRefresh.Click += btnRefresh_Click;

            pnlToolbar.Controls.AddRange(new System.Windows.Forms.Control[]
                { lblSearch, txtSearch, btnAdd, btnEdit, btnDelete, btnRefresh });

            // Count label
            lblCount = new System.Windows.Forms.Label
            {
                AutoSize  = true,
                Location  = new System.Drawing.Point(20, 116),
                ForeColor = System.Drawing.Color.FromArgb(148, 163, 184),
                Font      = new System.Drawing.Font("Segoe UI", 9f)
            };

            // Grid
            dgvStudents = new System.Windows.Forms.DataGridView
            {
                Location  = new System.Drawing.Point(20, 138),
                Size      = new System.Drawing.Size(980, 560),
                Anchor    = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
            };

            this.Controls.AddRange(new System.Windows.Forms.Control[]
                { lblTitle, pnlToolbar, lblCount, dgvStudents });
        }
    }
}

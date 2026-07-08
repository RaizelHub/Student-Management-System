namespace StudentAttendanceSysttem.Forms
{
    partial class AttendanceForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label          lblTitle, lblDate, lblSelectedStudent, lblStatus, lblSearchHdr, lblAttHdr;
        private System.Windows.Forms.TextBox        txtStudentSearch;
        private System.Windows.Forms.DataGridView   dgvSearch, dgvAttendance;
        private System.Windows.Forms.Button         btnTimeIn, btnTimeOut, btnAbsent, btnRefresh;
        private System.Windows.Forms.Panel          pnlLeft, pnlRight;

        protected override void Dispose(bool d) { if (d && components != null) components.Dispose(); base.Dispose(d); }

        private void InitializeComponent()
        {
            this.Text = "Attendance"; this.BackColor = System.Drawing.Color.FromArgb(18,18,30); this.Dock = DockStyle.Fill; this.Load += AttendanceForm_Load;

            lblTitle = new System.Windows.Forms.Label { Text="✅  Attendance Recording", Font=new System.Drawing.Font("Segoe UI",16f,System.Drawing.FontStyle.Bold), ForeColor=System.Drawing.Color.White, AutoSize=true, Location=new System.Drawing.Point(20,16) };
            lblDate  = new System.Windows.Forms.Label { AutoSize=true, Location=new System.Drawing.Point(20,46), ForeColor=System.Drawing.Color.FromArgb(148,163,184), Font=new System.Drawing.Font("Segoe UI",10f) };

            // Left panel — student search + actions
            pnlLeft = new System.Windows.Forms.Panel { Location=new System.Drawing.Point(20,70), Size=new System.Drawing.Size(340,660), BackColor=System.Drawing.Color.FromArgb(28,28,44) };

            lblSearchHdr = new System.Windows.Forms.Label { Text="Search Student", AutoSize=true, Location=new System.Drawing.Point(12,12), ForeColor=System.Drawing.Color.FromArgb(148,163,184) };
            txtStudentSearch = new System.Windows.Forms.TextBox { Location=new System.Drawing.Point(12,32), Size=new System.Drawing.Size(316,30), BackColor=System.Drawing.Color.FromArgb(35,35,55), ForeColor=System.Drawing.Color.White, BorderStyle=System.Windows.Forms.BorderStyle.FixedSingle, PlaceholderText="Name or student number..." };
            txtStudentSearch.TextChanged += txtStudentSearch_TextChanged;

            dgvSearch = new System.Windows.Forms.DataGridView { Location=new System.Drawing.Point(12,68), Size=new System.Drawing.Size(316,200) };
            dgvSearch.CellClick += dgvSearch_CellClick;

            lblSelectedStudent = new System.Windows.Forms.Label { Text="Selected: —", AutoSize=true, Location=new System.Drawing.Point(12,278), ForeColor=System.Drawing.Color.FromArgb(99,102,241), Font=new System.Drawing.Font("Segoe UI",10f,System.Drawing.FontStyle.Bold) };

            lblStatus = new System.Windows.Forms.Label { Location=new System.Drawing.Point(12,306), Size=new System.Drawing.Size(316,36), Visible=false, TextAlign=System.Drawing.ContentAlignment.MiddleLeft, Font=new System.Drawing.Font("Segoe UI",9f) };

            btnTimeIn  = new System.Windows.Forms.Button { Text="⏰ TIME IN",  Location=new System.Drawing.Point(12,356), Size=new System.Drawing.Size(316,52), Font=new System.Drawing.Font("Segoe UI",13f,System.Drawing.FontStyle.Bold) };
            btnTimeOut = new System.Windows.Forms.Button { Text="🏁 TIME OUT", Location=new System.Drawing.Point(12,418), Size=new System.Drawing.Size(316,52), Font=new System.Drawing.Font("Segoe UI",13f,System.Drawing.FontStyle.Bold) };
            btnAbsent  = new System.Windows.Forms.Button { Text="❌ MARK ABSENT", Location=new System.Drawing.Point(12,480), Size=new System.Drawing.Size(316,52), Font=new System.Drawing.Font("Segoe UI",11f,System.Drawing.FontStyle.Bold) };

            btnTimeIn.Click  += btnTimeIn_Click;
            btnTimeOut.Click += btnTimeOut_Click;
            btnAbsent.Click  += btnAbsent_Click;

            pnlLeft.Controls.AddRange(new System.Windows.Forms.Control[] { lblSearchHdr, txtStudentSearch, dgvSearch, lblSelectedStudent, lblStatus, btnTimeIn, btnTimeOut, btnAbsent });

            // Right panel — today's attendance grid
            pnlRight = new System.Windows.Forms.Panel { Location=new System.Drawing.Point(374,70), Size=new System.Drawing.Size(720,660), BackColor=System.Drawing.Color.Transparent };

            lblAttHdr = new System.Windows.Forms.Label { Text="Today's Attendance", Font=new System.Drawing.Font("Segoe UI",12f,System.Drawing.FontStyle.Bold), ForeColor=System.Drawing.Color.White, AutoSize=true, Location=new System.Drawing.Point(0,0) };
            btnRefresh = new System.Windows.Forms.Button { Text="🔄 Refresh", Location=new System.Drawing.Point(600,0), Size=new System.Drawing.Size(110,28) };
            btnRefresh.Click += btnRefresh_Click;

            dgvAttendance = new System.Windows.Forms.DataGridView { Location=new System.Drawing.Point(0,36), Size=new System.Drawing.Size(720,624), Anchor=AnchorStyles.Top|AnchorStyles.Left|AnchorStyles.Right|AnchorStyles.Bottom };

            pnlRight.Controls.AddRange(new System.Windows.Forms.Control[] { lblAttHdr, btnRefresh, dgvAttendance });

            this.Controls.AddRange(new System.Windows.Forms.Control[] { lblTitle, lblDate, pnlLeft, pnlRight });
        }
    }
}

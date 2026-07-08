namespace StudentAttendanceSysttem.Forms
{
    partial class ReportsForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle, lblCount;
        private System.Windows.Forms.ComboBox cmbReportType;
        private System.Windows.Forms.DateTimePicker dtpFrom, dtpTo;
        private System.Windows.Forms.Button btnGenerate, btnExportPdf, btnExportExcel;
        private System.Windows.Forms.DataGridView dgvReport;
        private System.Windows.Forms.Panel pnlToolbar;

        protected override void Dispose(bool d) { if (d && components != null) components.Dispose(); base.Dispose(d); }

        private void InitializeComponent()
        {
            this.Text = "Reports"; this.BackColor = System.Drawing.Color.FromArgb(18,18,30); this.Dock = DockStyle.Fill; this.Load += ReportsForm_Load;

            lblTitle = new System.Windows.Forms.Label { Text="📊  Reports", Font=new System.Drawing.Font("Segoe UI",16f,System.Drawing.FontStyle.Bold), ForeColor=System.Drawing.Color.White, AutoSize=true, Location=new System.Drawing.Point(20,16) };

            pnlToolbar = new System.Windows.Forms.Panel { Location=new System.Drawing.Point(20,55), Size=new System.Drawing.Size(1000,60), BackColor=System.Drawing.Color.FromArgb(28,28,44) };

            var l1 = new System.Windows.Forms.Label { Text="Report Type", AutoSize=true, Location=new System.Drawing.Point(10,8), ForeColor=System.Drawing.Color.FromArgb(148,163,184) };
            cmbReportType = new System.Windows.Forms.ComboBox { Location=new System.Drawing.Point(10,26), Size=new System.Drawing.Size(180,28), DropDownStyle=System.Windows.Forms.ComboBoxStyle.DropDownList };
            cmbReportType.Items.AddRange(new object[] { "Daily Attendance","Weekly Attendance","Monthly Attendance","Date Range" });
            cmbReportType.SelectedIndex = 0;

            var l2 = new System.Windows.Forms.Label { Text="From", AutoSize=true, Location=new System.Drawing.Point(200,8), ForeColor=System.Drawing.Color.FromArgb(148,163,184) };
            dtpFrom = new System.Windows.Forms.DateTimePicker { Location=new System.Drawing.Point(200,26), Size=new System.Drawing.Size(150,28), Format=System.Windows.Forms.DateTimePickerFormat.Short };
            var l3 = new System.Windows.Forms.Label { Text="To", AutoSize=true, Location=new System.Drawing.Point(360,8), ForeColor=System.Drawing.Color.FromArgb(148,163,184) };
            dtpTo = new System.Windows.Forms.DateTimePicker { Location=new System.Drawing.Point(360,26), Size=new System.Drawing.Size(150,28), Format=System.Windows.Forms.DateTimePickerFormat.Short };

            btnGenerate    = new System.Windows.Forms.Button { Text="📋 Generate",    Location=new System.Drawing.Point(526,20), Size=new System.Drawing.Size(110,30) };
            btnExportPdf   = new System.Windows.Forms.Button { Text="📄 Export PDF",  Location=new System.Drawing.Point(648,20), Size=new System.Drawing.Size(120,30) };
            btnExportExcel = new System.Windows.Forms.Button { Text="📊 Export Excel",Location=new System.Drawing.Point(780,20), Size=new System.Drawing.Size(130,30) };

            btnGenerate.Click    += btnGenerate_Click;
            btnExportPdf.Click   += btnExportPdf_Click;
            btnExportExcel.Click += btnExportExcel_Click;

            pnlToolbar.Controls.AddRange(new System.Windows.Forms.Control[] { l1,cmbReportType,l2,dtpFrom,l3,dtpTo,btnGenerate,btnExportPdf,btnExportExcel });

            lblCount = new System.Windows.Forms.Label { AutoSize=true, Location=new System.Drawing.Point(20,125), ForeColor=System.Drawing.Color.FromArgb(148,163,184) };

            dgvReport = new System.Windows.Forms.DataGridView { Location=new System.Drawing.Point(20,148), Size=new System.Drawing.Size(1000,560), Anchor=AnchorStyles.Top|AnchorStyles.Left|AnchorStyles.Right|AnchorStyles.Bottom };

            this.Controls.AddRange(new System.Windows.Forms.Control[] { lblTitle, pnlToolbar, lblCount, dgvReport });
        }
    }
}

namespace StudentAttendanceSysttem.Forms
{
    partial class AuditLogsForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle, lblCount;
        private System.Windows.Forms.DateTimePicker dtpFrom, dtpTo;
        private System.Windows.Forms.ComboBox cmbAction;
        private System.Windows.Forms.Button btnFilter, btnClear;
        private System.Windows.Forms.DataGridView dgvLogs;
        private System.Windows.Forms.Panel pnlFilter;
        protected override void Dispose(bool d) { if (d && components != null) components.Dispose(); base.Dispose(d); }
        private void InitializeComponent()
        {
            this.Text = "Audit Logs"; this.BackColor = System.Drawing.Color.FromArgb(18,18,30); this.Dock = DockStyle.Fill; this.Load += AuditLogsForm_Load;
            lblTitle = new System.Windows.Forms.Label { Text="📋  Audit Logs", Font=new System.Drawing.Font("Segoe UI",16f,System.Drawing.FontStyle.Bold), ForeColor=System.Drawing.Color.White, AutoSize=true, Location=new System.Drawing.Point(20,16) };
            pnlFilter = new System.Windows.Forms.Panel { Location=new System.Drawing.Point(20,55), Size=new System.Drawing.Size(960,55), BackColor=System.Drawing.Color.FromArgb(28,28,44) };
            var l1=new System.Windows.Forms.Label{Text="From",AutoSize=true,Location=new System.Drawing.Point(10,8),ForeColor=System.Drawing.Color.FromArgb(148,163,184)};
            dtpFrom=new System.Windows.Forms.DateTimePicker{Location=new System.Drawing.Point(10,26),Size=new System.Drawing.Size(150,26),Format=System.Windows.Forms.DateTimePickerFormat.Short};
            var l2=new System.Windows.Forms.Label{Text="To",AutoSize=true,Location=new System.Drawing.Point(170,8),ForeColor=System.Drawing.Color.FromArgb(148,163,184)};
            dtpTo=new System.Windows.Forms.DateTimePicker{Location=new System.Drawing.Point(170,26),Size=new System.Drawing.Size(150,26),Format=System.Windows.Forms.DateTimePickerFormat.Short};
            var l3=new System.Windows.Forms.Label{Text="Action",AutoSize=true,Location=new System.Drawing.Point(330,8),ForeColor=System.Drawing.Color.FromArgb(148,163,184)};
            cmbAction=new System.Windows.Forms.ComboBox{Location=new System.Drawing.Point(330,26),Size=new System.Drawing.Size(180,26),DropDownStyle=System.Windows.Forms.ComboBoxStyle.DropDownList};
            cmbAction.Items.AddRange(new object[]{"All Actions","LOGIN","LOGOUT","LOGIN_FAILED","ADD_STUDENT","UPDATE_STUDENT","DELETE_STUDENT","TIME_IN","TIME_OUT","MARK_ABSENT","ADD_USER","UPDATE_USER","DELETE_USER","RESET_PASSWORD","ADD_COURSE","ADD_SECTION","BACKUP","RESTORE"});
            cmbAction.SelectedIndex=0;
            btnFilter=new System.Windows.Forms.Button{Text="🔍 Filter",Location=new System.Drawing.Point(526,20),Size=new System.Drawing.Size(90,28)};
            btnClear =new System.Windows.Forms.Button{Text="✖ Clear",  Location=new System.Drawing.Point(626,20),Size=new System.Drawing.Size(90,28)};
            btnFilter.Click+=btnFilter_Click; btnClear.Click+=btnClear_Click;
            pnlFilter.Controls.AddRange(new System.Windows.Forms.Control[]{l1,dtpFrom,l2,dtpTo,l3,cmbAction,btnFilter,btnClear});
            lblCount=new System.Windows.Forms.Label{AutoSize=true,Location=new System.Drawing.Point(20,118),ForeColor=System.Drawing.Color.FromArgb(148,163,184)};
            dgvLogs=new System.Windows.Forms.DataGridView{Location=new System.Drawing.Point(20,140),Size=new System.Drawing.Size(960,560),Anchor=AnchorStyles.Top|AnchorStyles.Left|AnchorStyles.Right|AnchorStyles.Bottom};
            this.Controls.AddRange(new System.Windows.Forms.Control[]{lblTitle,pnlFilter,lblCount,dgvLogs});
        }
    }
}

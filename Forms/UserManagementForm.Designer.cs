namespace StudentAttendanceSysttem.Forms
{
    partial class UserManagementForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtUsername, txtFirstName, txtEmail, txtPassword;
        private System.Windows.Forms.ComboBox cmbRole;
        private System.Windows.Forms.CheckBox chkActive;
        private System.Windows.Forms.Button btnSave, btnDelete, btnReset, btnNew;
        private System.Windows.Forms.DataGridView dgvUsers;
        private System.Windows.Forms.Panel pnlForm;
        protected override void Dispose(bool d) { if (d && components != null) components.Dispose(); base.Dispose(d); }
        private void InitializeComponent()
        {
            this.Text = "User Management"; this.BackColor = System.Drawing.Color.FromArgb(18,18,30); this.Dock = DockStyle.Fill; this.Load += UserManagementForm_Load;
            lblTitle = new System.Windows.Forms.Label { Text="👥  User Management", Font=new System.Drawing.Font("Segoe UI",16f,System.Drawing.FontStyle.Bold), ForeColor=System.Drawing.Color.White, AutoSize=true, Location=new System.Drawing.Point(20,16) };
            pnlForm = new System.Windows.Forms.Panel { Location=new System.Drawing.Point(20,56), Size=new System.Drawing.Size(920,110), BackColor=System.Drawing.Color.FromArgb(28,28,44) };
            System.Windows.Forms.Control[] pfc = {
                Lbl("Username",10,8), Tb(out txtUsername,10,26,160,"username"),
                Lbl("First Name",180,8), Tb(out txtFirstName,180,26,140,""),
                Lbl("Email",330,8), Tb(out txtEmail,330,26,200,""),
                Lbl("Password",540,8), Pw(out txtPassword,540,26,150),
                Lbl("Role",700,8), Rb(out cmbRole,700,26),
                Lbl("Active",840,8), Cb(out chkActive,840,30)
            };
            pnlForm.Controls.AddRange(pfc);
            btnNew    = new System.Windows.Forms.Button { Text="🆕 New",          Location=new System.Drawing.Point(10,72),  Size=new System.Drawing.Size(90,30) };
            btnSave   = new System.Windows.Forms.Button { Text="💾 Save",         Location=new System.Drawing.Point(110,72), Size=new System.Drawing.Size(90,30) };
            btnDelete = new System.Windows.Forms.Button { Text="🗑️ Delete",      Location=new System.Drawing.Point(210,72), Size=new System.Drawing.Size(90,30) };
            btnReset  = new System.Windows.Forms.Button { Text="🔑 Reset Pwd",   Location=new System.Drawing.Point(310,72), Size=new System.Drawing.Size(110,30) };
            btnNew.Click    += btnNew_Click;
            btnSave.Click   += btnSave_Click;
            btnDelete.Click += btnDelete_Click;
            btnReset.Click  += btnReset_Click;
            pnlForm.Controls.AddRange(new System.Windows.Forms.Control[] { btnNew, btnSave, btnDelete, btnReset });
            dgvUsers = new System.Windows.Forms.DataGridView { Location=new System.Drawing.Point(20,178), Size=new System.Drawing.Size(920,530), Anchor=AnchorStyles.Top|AnchorStyles.Left|AnchorStyles.Right|AnchorStyles.Bottom };
            dgvUsers.CellClick += dgvUsers_CellClick;
            this.Controls.AddRange(new System.Windows.Forms.Control[] { lblTitle, pnlForm, dgvUsers });
        }
        private static System.Windows.Forms.Label Lbl(string t,int x,int y) => new System.Windows.Forms.Label { Text=t,AutoSize=true,Location=new System.Drawing.Point(x,y),ForeColor=System.Drawing.Color.FromArgb(148,163,184),Font=new System.Drawing.Font("Segoe UI",8.5f) };
        private static System.Windows.Forms.Control Tb(out System.Windows.Forms.TextBox tb,int x,int y,int w,string ph) { tb=new System.Windows.Forms.TextBox{Location=new System.Drawing.Point(x,y),Size=new System.Drawing.Size(w,26),BackColor=System.Drawing.Color.FromArgb(35,35,55),ForeColor=System.Drawing.Color.White,BorderStyle=System.Windows.Forms.BorderStyle.FixedSingle,PlaceholderText=ph};return tb; }
        private static System.Windows.Forms.Control Pw(out System.Windows.Forms.TextBox tb,int x,int y,int w) { tb=new System.Windows.Forms.TextBox{Location=new System.Drawing.Point(x,y),Size=new System.Drawing.Size(w,26),UseSystemPasswordChar=true,BackColor=System.Drawing.Color.FromArgb(35,35,55),ForeColor=System.Drawing.Color.White,BorderStyle=System.Windows.Forms.BorderStyle.FixedSingle};return tb; }
        private static System.Windows.Forms.Control Rb(out System.Windows.Forms.ComboBox cb,int x,int y) { cb=new System.Windows.Forms.ComboBox{Location=new System.Drawing.Point(x,y),Size=new System.Drawing.Size(130,26),DropDownStyle=System.Windows.Forms.ComboBoxStyle.DropDownList};cb.Items.AddRange(new object[]{"Administrator","Staff"});cb.SelectedIndex=1;return cb; }
        private static System.Windows.Forms.Control Cb(out System.Windows.Forms.CheckBox chk,int x,int y) { chk=new System.Windows.Forms.CheckBox{Location=new System.Drawing.Point(x,y),Checked=true,AutoSize=true};return chk; }
    }
}

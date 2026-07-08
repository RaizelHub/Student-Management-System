namespace StudentAttendanceSysttem.Forms
{
    partial class SectionForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle, lblCount;
        private System.Windows.Forms.TextBox txtSectionName, txtYearLevel, txtSchoolYear;
        private System.Windows.Forms.ComboBox cmbCourse;
        private System.Windows.Forms.Button btnAdd, btnEdit, btnDelete;
        private System.Windows.Forms.DataGridView dgvSections;
        private System.Windows.Forms.Panel pnlForm;
        protected override void Dispose(bool d) { if (d && components != null) components.Dispose(); base.Dispose(d); }
        private void InitializeComponent()
        {
            this.Text = "Section Management"; this.BackColor = System.Drawing.Color.FromArgb(18,18,30); this.Dock = DockStyle.Fill; this.Load += SectionForm_Load;
            lblTitle = new System.Windows.Forms.Label { Text="🏫  Section Management", Font=new System.Drawing.Font("Segoe UI",16f,System.Drawing.FontStyle.Bold), ForeColor=System.Drawing.Color.White, AutoSize=true, Location=new System.Drawing.Point(20,20) };
            pnlForm = new System.Windows.Forms.Panel { Location=new System.Drawing.Point(20,60), Size=new System.Drawing.Size(680,130), BackColor=System.Drawing.Color.FromArgb(28,28,44) };
            var l1 = MkLbl("Section Name *",10,10); txtSectionName = MkTb(10,28,180);
            var l2 = MkLbl("Course",200,10);
            cmbCourse = new System.Windows.Forms.ComboBox { Location=new System.Drawing.Point(200,28), Size=new System.Drawing.Size(180,28), DropDownStyle=System.Windows.Forms.ComboBoxStyle.DropDownList };
            var l3 = MkLbl("Year Level",10,70); txtYearLevel = MkTb(10,88,120);
            var l4 = MkLbl("School Year",140,70); txtSchoolYear = MkTb(140,88,140);
            btnAdd    = new System.Windows.Forms.Button { Text="➕ Add",    Location=new System.Drawing.Point(400,20), Size=new System.Drawing.Size(80,32) };
            btnEdit   = new System.Windows.Forms.Button { Text="✏️ Edit",   Location=new System.Drawing.Point(490,20), Size=new System.Drawing.Size(80,32) };
            btnDelete = new System.Windows.Forms.Button { Text="🗑️ Delete", Location=new System.Drawing.Point(400,62), Size=new System.Drawing.Size(170,32) };
            btnAdd.Click += btnAdd_Click; btnEdit.Click += btnEdit_Click; btnDelete.Click += btnDelete_Click;
            pnlForm.Controls.AddRange(new System.Windows.Forms.Control[] { l1,txtSectionName,l2,cmbCourse,l3,txtYearLevel,l4,txtSchoolYear,btnAdd,btnEdit,btnDelete });
            lblCount = new System.Windows.Forms.Label { AutoSize=true, Location=new System.Drawing.Point(20,200), ForeColor=System.Drawing.Color.FromArgb(148,163,184) };
            dgvSections = new System.Windows.Forms.DataGridView { Location=new System.Drawing.Point(20,222), Size=new System.Drawing.Size(680,450), Anchor=AnchorStyles.Top|AnchorStyles.Left|AnchorStyles.Right|AnchorStyles.Bottom };
            dgvSections.CellClick += dgvSections_CellClick;
            this.Controls.AddRange(new System.Windows.Forms.Control[] { lblTitle, pnlForm, lblCount, dgvSections });
        }
        private static System.Windows.Forms.Label MkLbl(string t,int x,int y) => new System.Windows.Forms.Label { Text=t,AutoSize=true,Location=new System.Drawing.Point(x,y),ForeColor=System.Drawing.Color.FromArgb(148,163,184) };
        private static System.Windows.Forms.TextBox MkTb(int x,int y,int w) => new System.Windows.Forms.TextBox { Location=new System.Drawing.Point(x,y),Size=new System.Drawing.Size(w,28),BackColor=System.Drawing.Color.FromArgb(35,35,55),ForeColor=System.Drawing.Color.White,BorderStyle=System.Windows.Forms.BorderStyle.FixedSingle };
    }
}

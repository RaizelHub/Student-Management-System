namespace StudentAttendanceSysttem.Forms
{
    partial class CourseForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label          lblTitle, lblCount;
        private System.Windows.Forms.TextBox        txtCourseCode, txtCourseName, txtDescription;
        private System.Windows.Forms.Button         btnAdd, btnEdit, btnDelete;
        private System.Windows.Forms.DataGridView   dgvCourses;
        private System.Windows.Forms.Panel          pnlForm;

        protected override void Dispose(bool d) { if (d && components != null) components.Dispose(); base.Dispose(d); }

        private void InitializeComponent()
        {
            this.Text = "Course Management"; this.BackColor = System.Drawing.Color.FromArgb(18,18,30);
            this.Dock = DockStyle.Fill; this.Load += CourseForm_Load;

            lblTitle = new System.Windows.Forms.Label { Text = "📚  Course Management", Font = new System.Drawing.Font("Segoe UI",16f,System.Drawing.FontStyle.Bold), ForeColor = System.Drawing.Color.White, AutoSize = true, Location = new System.Drawing.Point(20,20) };

            pnlForm = new System.Windows.Forms.Panel { Location = new System.Drawing.Point(20,60), Size = new System.Drawing.Size(640,120), BackColor = System.Drawing.Color.FromArgb(28,28,44) };

            var lCC = new System.Windows.Forms.Label { Text="Course Code", AutoSize=true, Location=new System.Drawing.Point(10,10), ForeColor=System.Drawing.Color.FromArgb(148,163,184) };
            txtCourseCode = new System.Windows.Forms.TextBox { Location=new System.Drawing.Point(10,28), Size=new System.Drawing.Size(140,28), BackColor=System.Drawing.Color.FromArgb(35,35,55), ForeColor=System.Drawing.Color.White, BorderStyle=System.Windows.Forms.BorderStyle.FixedSingle, PlaceholderText="e.g. BSCS" };
            var lCN = new System.Windows.Forms.Label { Text="Course Name *", AutoSize=true, Location=new System.Drawing.Point(160,10), ForeColor=System.Drawing.Color.FromArgb(148,163,184) };
            txtCourseName = new System.Windows.Forms.TextBox { Location=new System.Drawing.Point(160,28), Size=new System.Drawing.Size(220,28), BackColor=System.Drawing.Color.FromArgb(35,35,55), ForeColor=System.Drawing.Color.White, BorderStyle=System.Windows.Forms.BorderStyle.FixedSingle };
            var lD = new System.Windows.Forms.Label { Text="Description", AutoSize=true, Location=new System.Drawing.Point(10,64), ForeColor=System.Drawing.Color.FromArgb(148,163,184) };
            txtDescription = new System.Windows.Forms.TextBox { Location=new System.Drawing.Point(10,82), Size=new System.Drawing.Size(370,28), BackColor=System.Drawing.Color.FromArgb(35,35,55), ForeColor=System.Drawing.Color.White, BorderStyle=System.Windows.Forms.BorderStyle.FixedSingle };

            btnAdd    = new System.Windows.Forms.Button { Text="➕ Add",    Location=new System.Drawing.Point(400,20), Size=new System.Drawing.Size(70,32) };
            btnEdit   = new System.Windows.Forms.Button { Text="✏️ Edit",   Location=new System.Drawing.Point(480,20), Size=new System.Drawing.Size(70,32) };
            btnDelete = new System.Windows.Forms.Button { Text="🗑️ Delete", Location=new System.Drawing.Point(400,62), Size=new System.Drawing.Size(150,32) };

            btnAdd.Click    += btnAdd_Click;
            btnEdit.Click   += btnEdit_Click;
            btnDelete.Click += btnDelete_Click;

            pnlForm.Controls.AddRange(new System.Windows.Forms.Control[] { lCC,txtCourseCode,lCN,txtCourseName,lD,txtDescription,btnAdd,btnEdit,btnDelete });

            lblCount = new System.Windows.Forms.Label { AutoSize=true, Location=new System.Drawing.Point(20,190), ForeColor=System.Drawing.Color.FromArgb(148,163,184), Font=new System.Drawing.Font("Segoe UI",9f) };

            dgvCourses = new System.Windows.Forms.DataGridView { Location=new System.Drawing.Point(20,210), Size=new System.Drawing.Size(640,460), Anchor=AnchorStyles.Top|AnchorStyles.Left|AnchorStyles.Right|AnchorStyles.Bottom };
            dgvCourses.CellClick += dgvCourses_CellClick;

            this.Controls.AddRange(new System.Windows.Forms.Control[] { lblTitle, pnlForm, lblCount, dgvCourses });
        }
    }
}

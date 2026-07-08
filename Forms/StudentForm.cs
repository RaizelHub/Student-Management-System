using System;
using System.Data;
using System.Windows.Forms;
using StudentAttendanceSysttem.Database;
using StudentAttendanceSysttem.Repositories;
using StudentAttendanceSysttem.Services;
using StudentAttendanceSysttem.Utilities;

namespace StudentAttendanceSysttem.Forms
{
    public partial class StudentForm : Form
    {
        private readonly StudentService _service;
        private readonly CourseService  _courseService;
        private readonly SectionService _sectionService;
        private readonly AuditService   _auditService;
        private readonly DatabaseConnection _db;

        public StudentForm()
        {
            InitializeComponent();
            _db             = new DatabaseConnection();
            _auditService   = new AuditService(_db);
            _service        = new StudentService(new StudentRepository(_db), _auditService);
            _courseService  = new CourseService(new CourseRepository(_db), _auditService);
            _sectionService = new SectionService(new SectionRepository(_db), _auditService);
        }

        private void StudentForm_Load(object sender, EventArgs e)
        {
            ThemeManager.ApplyTheme(this);
            ThemeManager.StyleDataGridView(dgvStudents);
            ThemeManager.StylePrimaryButton(btnAdd);
            ThemeManager.StyleSecondaryButton(btnEdit);
            ThemeManager.StyleDangerButton(btnDelete);
            ThemeManager.StyleSecondaryButton(btnRefresh);

            LoadStudents();
        }

        private void LoadStudents()
        {
            try
            {
                var dt = _service.GetStudentsAsTable();
                dgvStudents.DataSource = dt;
                FormatGrid();
                lblCount.Text = $"Total: {dt.Rows.Count} students";
            }
            catch (Exception ex) { NotificationHelper.ShowError(ex.Message); }
        }

        private void FormatGrid()
        {
            if (dgvStudents.Columns.Contains("id"))
                dgvStudents.Columns["id"]!.Visible = false;
            var headers = new[] {
                ("student_number","Student No."),("full_name","Full Name"),
                ("gender","Gender"),("year_level","Year"),
                ("course_name","Course"),("section_name","Section"),
                ("email","Email"),("contact_number","Contact")
            };
            foreach (var (col, hdr) in headers)
                if (dgvStudents.Columns.Contains(col))
                    dgvStudents.Columns[col]!.HeaderText = hdr;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            var dt = _service.SearchStudents(txtSearch.Text.Trim());
            dgvStudents.DataSource = dt;
            FormatGrid();
            lblCount.Text = $"Found: {dt.Rows.Count} students";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var form = new StudentDetailsForm(null, _db);
            if (form.ShowDialog(this) == DialogResult.OK) LoadStudents();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvStudents.CurrentRow == null) { NotificationHelper.ShowWarning("Select a student first."); return; }
            int id = Convert.ToInt32(dgvStudents.CurrentRow.Cells["id"].Value);
            var student = _service.GetStudentById(id);
            if (student == null) { NotificationHelper.ShowError("Student not found."); return; }
            var form = new StudentDetailsForm(student, _db);
            if (form.ShowDialog(this) == DialogResult.OK) LoadStudents();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvStudents.CurrentRow == null) { NotificationHelper.ShowWarning("Select a student first."); return; }
            if (!SessionManager.Instance.IsAdmin) { NotificationHelper.ShowError("Only Administrators can delete students."); return; }
            int    id   = Convert.ToInt32(dgvStudents.CurrentRow.Cells["id"].Value);
            string name = dgvStudents.CurrentRow.Cells["full_name"].Value?.ToString() ?? "";
            if (!NotificationHelper.Confirm($"Deactivate student '{name}'?")) return;
            var (ok, msg) = _service.DeleteStudent(id, name);
            if (ok) { NotificationHelper.ShowSuccess(msg); LoadStudents(); }
            else      NotificationHelper.ShowError(msg);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            LoadStudents();
        }
    }
}

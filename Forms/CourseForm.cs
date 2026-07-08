using System;
using System.Windows.Forms;
using StudentAttendanceSysttem.Database;
using StudentAttendanceSysttem.Models;
using StudentAttendanceSysttem.Repositories;
using StudentAttendanceSysttem.Services;
using StudentAttendanceSysttem.Utilities;

namespace StudentAttendanceSysttem.Forms
{
    public partial class CourseForm : Form
    {
        private readonly CourseService _service;
        private readonly AuditService  _audit;
        private readonly DatabaseConnection _db;

        public CourseForm()
        {
            InitializeComponent();
            _db      = new DatabaseConnection();
            _audit   = new AuditService(_db);
            _service = new CourseService(new CourseRepository(_db), _audit);
        }

        private void CourseForm_Load(object sender, EventArgs e)
        {
            ThemeManager.ApplyTheme(this);
            ThemeManager.StyleDataGridView(dgvCourses);
            ThemeManager.StylePrimaryButton(btnAdd);
            ThemeManager.StyleSecondaryButton(btnEdit);
            ThemeManager.StyleDangerButton(btnDelete);
            Load_Grid();
        }

        private void Load_Grid()
        {
            dgvCourses.DataSource = _service.GetCoursesTable();
            lblCount.Text = $"Total: {dgvCourses.RowCount} courses";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs(out var c)) return;
            var (ok, msg, _) = _service.AddCourse(c);
            if (ok) { ClearInputs(); Load_Grid(); NotificationHelper.ShowSuccess(msg); }
            else      NotificationHelper.ShowError(msg);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvCourses.CurrentRow == null) { NotificationHelper.ShowWarning("Select a course."); return; }
            if (!ValidateInputs(out var c)) return;
            c.Id = Convert.ToInt32(dgvCourses.CurrentRow.Cells["id"].Value);
            var (ok, msg) = _service.UpdateCourse(c);
            if (ok) { ClearInputs(); Load_Grid(); NotificationHelper.ShowSuccess(msg); }
            else      NotificationHelper.ShowError(msg);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvCourses.CurrentRow == null) { NotificationHelper.ShowWarning("Select a course."); return; }
            if (!SessionManager.Instance.IsAdmin) { NotificationHelper.ShowError("Admin only."); return; }
            int    id   = Convert.ToInt32(dgvCourses.CurrentRow.Cells["id"].Value);
            string name = dgvCourses.CurrentRow.Cells["course_name"].Value?.ToString() ?? "";
            if (!NotificationHelper.Confirm($"Delete course '{name}'?")) return;
            var (ok, msg) = _service.DeleteCourse(id, name);
            if (ok) { ClearInputs(); Load_Grid(); NotificationHelper.ShowSuccess(msg); }
            else      NotificationHelper.ShowError(msg);
        }

        private void dgvCourses_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvCourses.Rows[e.RowIndex];
            txtCourseCode.Text = row.Cells["course_code"].Value?.ToString();
            txtCourseName.Text = row.Cells["course_name"].Value?.ToString();
            txtDescription.Text = row.Cells["description"].Value?.ToString();
        }

        private bool ValidateInputs(out Course c)
        {
            c = new Course
            {
                CourseCode  = txtCourseCode.Text.Trim(),
                CourseName  = txtCourseName.Text.Trim(),
                Description = txtDescription.Text.Trim()
            };
            if (string.IsNullOrWhiteSpace(c.CourseName))
            { NotificationHelper.ShowWarning("Course name is required."); return false; }
            return true;
        }

        private void ClearInputs()
        { txtCourseCode.Clear(); txtCourseName.Clear(); txtDescription.Clear(); }
    }
}

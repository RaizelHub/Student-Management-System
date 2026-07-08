using System;
using System.Windows.Forms;
using StudentAttendanceSysttem.Database;
using StudentAttendanceSysttem.Models;
using StudentAttendanceSysttem.Repositories;
using StudentAttendanceSysttem.Services;
using StudentAttendanceSysttem.Utilities;

namespace StudentAttendanceSysttem.Forms
{
    public partial class SectionForm : Form
    {
        private readonly SectionService _service;
        private readonly CourseService  _courseService;
        private readonly AuditService   _audit;
        private readonly DatabaseConnection _db;

        public SectionForm()
        {
            InitializeComponent();
            _db            = new DatabaseConnection();
            _audit         = new AuditService(_db);
            _service       = new SectionService(new SectionRepository(_db), _audit);
            _courseService = new CourseService(new CourseRepository(_db), _audit);
        }

        private void SectionForm_Load(object sender, EventArgs e)
        {
            ThemeManager.ApplyTheme(this);
            ThemeManager.StyleDataGridView(dgvSections);
            ThemeManager.StylePrimaryButton(btnAdd);
            ThemeManager.StyleSecondaryButton(btnEdit);
            ThemeManager.StyleDangerButton(btnDelete);
            LoadCourses();
            Load_Grid();
        }

        private void LoadCourses()
        {
            cmbCourse.Items.Clear();
            cmbCourse.Items.Add(new ComboItem(0, "— None —"));
            foreach (var c in _courseService.GetAllCourses())
                cmbCourse.Items.Add(new ComboItem(c.Id, c.CourseName));
            cmbCourse.DisplayMember = "Text";
            cmbCourse.SelectedIndex = 0;
        }

        private void Load_Grid()
        {
            dgvSections.DataSource = _service.GetSectionsTable();
            lblCount.Text = $"Total: {dgvSections.RowCount} sections";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!GetInputs(out var s)) return;
            var (ok, msg, _) = _service.AddSection(s);
            if (ok) { ClearInputs(); Load_Grid(); NotificationHelper.ShowSuccess(msg); }
            else      NotificationHelper.ShowError(msg);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvSections.CurrentRow == null) { NotificationHelper.ShowWarning("Select a section."); return; }
            if (!GetInputs(out var s)) return;
            s.Id = Convert.ToInt32(dgvSections.CurrentRow.Cells["id"].Value);
            var (ok, msg) = _service.UpdateSection(s);
            if (ok) { ClearInputs(); Load_Grid(); NotificationHelper.ShowSuccess(msg); }
            else      NotificationHelper.ShowError(msg);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvSections.CurrentRow == null) { NotificationHelper.ShowWarning("Select a section."); return; }
            int id = Convert.ToInt32(dgvSections.CurrentRow.Cells["id"].Value);
            string name = dgvSections.CurrentRow.Cells["section_name"].Value?.ToString() ?? "";
            if (!NotificationHelper.Confirm($"Delete section '{name}'?")) return;
            var (ok, msg) = _service.DeleteSection(id, name);
            if (ok) { ClearInputs(); Load_Grid(); NotificationHelper.ShowSuccess(msg); }
            else      NotificationHelper.ShowError(msg);
        }

        private void dgvSections_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvSections.Rows[e.RowIndex];
            txtSectionName.Text = row.Cells["section_name"].Value?.ToString();
            txtYearLevel.Text   = row.Cells["year_level"].Value?.ToString();
            txtSchoolYear.Text  = row.Cells["school_year"].Value?.ToString();
        }

        private bool GetInputs(out Section s)
        {
            s = new Section
            {
                SectionName = txtSectionName.Text.Trim(),
                CourseId    = (cmbCourse.SelectedItem as ComboItem)?.Value ?? 0,
                YearLevel   = txtYearLevel.Text.Trim(),
                SchoolYear  = txtSchoolYear.Text.Trim()
            };
            if (string.IsNullOrWhiteSpace(s.SectionName))
            { NotificationHelper.ShowWarning("Section name is required."); return false; }
            return true;
        }

        private void ClearInputs()
        { txtSectionName.Clear(); txtYearLevel.Clear(); txtSchoolYear.Clear(); cmbCourse.SelectedIndex = 0; }

        private sealed class ComboItem
        {
            public int Value; public string Text;
            public ComboItem(int v, string t) { Value = v; Text = t; }
            public override string ToString() => Text;
        }
    }
}

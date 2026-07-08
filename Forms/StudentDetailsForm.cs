using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using StudentAttendanceSysttem.Database;
using StudentAttendanceSysttem.Models;
using StudentAttendanceSysttem.Repositories;
using StudentAttendanceSysttem.Services;
using StudentAttendanceSysttem.Utilities;

namespace StudentAttendanceSysttem.Forms
{
    /// <summary>
    /// Add/Edit form for a single student record.
    /// Opened in dialog mode from StudentForm.
    /// </summary>
    public partial class StudentDetailsForm : Form
    {
        private readonly Student?        _student;   // null = new student
        private readonly StudentService  _service;
        private readonly CourseService   _courseService;
        private readonly SectionService  _sectionService;
        private readonly AuditService    _auditService;
        private string?  _photoPath;

        public StudentDetailsForm(Student? student, DatabaseConnection db)
        {
            InitializeComponent();
            _student        = student;
            _auditService   = new AuditService(db);
            _service        = new StudentService(new StudentRepository(db), _auditService);
            _courseService  = new CourseService(new CourseRepository(db), _auditService);
            _sectionService = new SectionService(new SectionRepository(db), _auditService);
        }

        private void StudentDetailsForm_Load(object sender, EventArgs e)
        {
            ThemeManager.ApplyTheme(this);
            ThemeManager.StylePrimaryButton(btnSave);
            ThemeManager.StyleSecondaryButton(btnCancel);

            this.Text = _student == null ? "Add New Student" : $"Edit Student — {_student.FullName}";
            lblTitle.Text = _student == null ? "Add New Student" : "Edit Student";

            LoadCourses();

            if (_student != null) PopulateFields();
        }

        private void LoadCourses()
        {
            cmbCourse.Items.Clear();
            cmbCourse.Items.Add(new ComboItem(0, "— Select Course —"));
            foreach (var c in _courseService.GetAllCourses())
                cmbCourse.Items.Add(new ComboItem(c.Id, c.CourseName));
            cmbCourse.DisplayMember = "Text";
            cmbCourse.ValueMember   = "Value";
            cmbCourse.SelectedIndex = 0;
        }

        private void cmbCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbSection.Items.Clear();
            cmbSection.Items.Add(new ComboItem(0, "— Select Section —"));
            if (cmbCourse.SelectedItem is ComboItem item && item.Value > 0)
            {
                foreach (var s in _sectionService.GetByCourse(item.Value))
                    cmbSection.Items.Add(new ComboItem(s.Id, s.SectionName));
            }
            cmbSection.DisplayMember = "Text";
            cmbSection.ValueMember   = "Value";
            cmbSection.SelectedIndex = 0;
        }

        private void PopulateFields()
        {
            if (_student == null) return;
            txtStudentNumber.Text = _student.StudentNumber;
            txtFirstName.Text     = _student.FirstName;
            txtMiddleName.Text    = _student.MiddleName;
            txtLastName.Text      = _student.LastName;
            cmbGender.Text        = _student.Gender;
            if (_student.BirthDate.HasValue) dtpBirthDate.Value = _student.BirthDate.Value;
            txtAddress.Text       = _student.Address;
            txtContact.Text       = _student.ContactNumber;
            txtEmail.Text         = _student.Email;
            txtYearLevel.Text     = _student.YearLevel;
            txtRfid.Text          = _student.RfidNumber;
            txtFingerprint.Text   = _student.FingerprintId;

            // Course
            foreach (ComboItem item in cmbCourse.Items)
                if (item.Value == _student.CourseId) { cmbCourse.SelectedItem = item; break; }

            // Section — populated after course selection
            BeginInvoke(new Action(() =>
            {
                foreach (ComboItem item in cmbSection.Items)
                    if (item.Value == _student.SectionId) { cmbSection.SelectedItem = item; break; }
            }));

            // Photo
            if (!string.IsNullOrEmpty(_student.PhotoPath) && File.Exists(_student.PhotoPath))
            {
                picPhoto.Image = Image.FromFile(_student.PhotoPath);
                _photoPath     = _student.PhotoPath;
            }
        }

        private void btnPhoto_Click(object sender, EventArgs e)
        {
            using var dlg = new OpenFileDialog
            {
                Title  = "Select Student Photo",
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp"
            };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                picPhoto.Image = Image.FromFile(dlg.FileName);
                _photoPath     = dlg.FileName;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var student = BuildStudent();
            bool isNew  = _student == null;

            if (isNew)
            {
                var (ok, msg, _) = _service.AddStudent(student);
                if (ok) { NotificationHelper.ShowSuccess(msg); DialogResult = DialogResult.OK; }
                else      NotificationHelper.ShowError(msg);
            }
            else
            {
                student.Id = _student!.Id;
                var (ok, msg) = _service.UpdateStudent(student);
                if (ok) { NotificationHelper.ShowSuccess(msg); DialogResult = DialogResult.OK; }
                else      NotificationHelper.ShowError(msg);
            }
        }

        private Student BuildStudent() => new()
        {
            StudentNumber = txtStudentNumber.Text.Trim(),
            FirstName     = txtFirstName.Text.Trim(),
            MiddleName    = txtMiddleName.Text.Trim(),
            LastName      = txtLastName.Text.Trim(),
            Gender        = cmbGender.Text,
            BirthDate     = dtpBirthDate.Checked ? dtpBirthDate.Value : null,
            Address       = txtAddress.Text.Trim(),
            ContactNumber = txtContact.Text.Trim(),
            Email         = txtEmail.Text.Trim(),
            CourseId      = (cmbCourse.SelectedItem as ComboItem)?.Value ?? 0,
            SectionId     = (cmbSection.SelectedItem as ComboItem)?.Value ?? 0,
            YearLevel     = txtYearLevel.Text.Trim(),
            RfidNumber    = txtRfid.Text.Trim(),
            FingerprintId = txtFingerprint.Text.Trim(),
            PhotoPath     = _photoPath
        };

        private void btnCancel_Click(object sender, EventArgs e) => DialogResult = DialogResult.Cancel;

        private sealed class ComboItem
        {
            public int    Value { get; }
            public string Text  { get; }
            public ComboItem(int v, string t) { Value = v; Text = t; }
            public override string ToString() => Text;
        }
    }
}

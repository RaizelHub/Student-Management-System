using System;
using System.Data;
using System.Windows.Forms;
using StudentAttendanceSysttem.Database;
using StudentAttendanceSysttem.Repositories;
using StudentAttendanceSysttem.Services;
using StudentAttendanceSysttem.Utilities;

namespace StudentAttendanceSysttem.Forms
{
    /// <summary>
    /// Attendance recording form.
    /// Supports Time In, Time Out, and Absent marking for the current date.
    /// Students are searched by name or student number.
    /// </summary>
    public partial class AttendanceForm : Form
    {
        private readonly AttendanceService _service;
        private readonly StudentService    _studentService;
        private readonly AuditService      _audit;
        private readonly DatabaseConnection _db;

        private int _selectedStudentId;
        private string _selectedStudentName = string.Empty;

        public AttendanceForm()
        {
            InitializeComponent();
            _db             = new DatabaseConnection();
            _audit          = new AuditService(_db);
            _service        = new AttendanceService(new AttendanceRepository(_db), _audit);
            _studentService = new StudentService(new StudentRepository(_db), _audit);
        }

        private void AttendanceForm_Load(object sender, EventArgs e)
        {
            ThemeManager.ApplyTheme(this);
            ThemeManager.StyleDataGridView(dgvAttendance);
            ThemeManager.StyleDataGridView(dgvSearch);
            ThemeManager.StylePrimaryButton(btnTimeIn);
            ThemeManager.StylePrimaryButton(btnTimeOut);
            ThemeManager.StyleDangerButton(btnAbsent);
            ThemeManager.StyleSecondaryButton(btnRefresh);

            btnTimeOut.BackColor = ThemeManager.Warning;
            lblDate.Text         = DateTime.Today.ToString("dddd, MMMM dd, yyyy");

            LoadTodayAttendance();
        }

        private void LoadTodayAttendance()
        {
            dgvAttendance.DataSource = _service.GetTodayAttendance();
            FormatAttGrid();
        }

        private void FormatAttGrid()
        {
            if (dgvAttendance.Columns.Contains("id")) dgvAttendance.Columns["id"]!.Visible = false;
            var headers = new[] {
                ("student_number","Student No."),("student_name","Name"),
                ("course_name","Course"),("section_name","Section"),
                ("date","Date"),("time_in","Time In"),("time_out","Time Out"),
                ("status","Status"),("remarks","Remarks")
            };
            foreach (var (col, hdr) in headers)
                if (dgvAttendance.Columns.Contains(col))
                    dgvAttendance.Columns[col]!.HeaderText = hdr;

            dgvAttendance.CellFormatting += (s, ev) =>
            {
                if (ev.RowIndex < 0 || dgvAttendance.Columns[ev.ColumnIndex].Name != "status") return;
                ev.CellStyle!.ForeColor = ev.Value?.ToString() switch
                {
                    "Present" => ThemeManager.Success,
                    "Absent"  => ThemeManager.Danger,
                    "Late"    => ThemeManager.Warning,
                    _         => ThemeManager.TextSecondary
                };
                ev.CellStyle.Font = ThemeManager.FontBold;
            };
        }

        // ── Student Search ────────────────────────────────────────────────────────
        private void txtStudentSearch_TextChanged(object sender, EventArgs e)
        {
            string q = txtStudentSearch.Text.Trim();
            if (q.Length < 2) { dgvSearch.DataSource = null; return; }
            dgvSearch.DataSource = _studentService.SearchStudents(q);
            if (dgvSearch.Columns.Contains("id")) dgvSearch.Columns["id"]!.Visible = false;
        }

        private void dgvSearch_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvSearch.Rows[e.RowIndex];
            _selectedStudentId   = Convert.ToInt32(row.Cells["id"].Value);
            _selectedStudentName = row.Cells["full_name"].Value?.ToString() ?? "";
            lblSelectedStudent.Text = $"Selected: {_selectedStudentName}";
        }

        // ── Actions ───────────────────────────────────────────────────────────────
        private void btnTimeIn_Click(object sender, EventArgs e)
        {
            if (_selectedStudentId == 0) { NotificationHelper.ShowWarning("Please search and select a student first."); return; }
            var (ok, msg, _) = _service.TimeIn(_selectedStudentId, _selectedStudentName);
            ShowStatus(msg, ok);
            if (ok) LoadTodayAttendance();
        }

        private void btnTimeOut_Click(object sender, EventArgs e)
        {
            if (_selectedStudentId == 0) { NotificationHelper.ShowWarning("Please search and select a student first."); return; }
            var (ok, msg) = _service.TimeOut(_selectedStudentId, _selectedStudentName);
            ShowStatus(msg, ok);
            if (ok) LoadTodayAttendance();
        }

        private void btnAbsent_Click(object sender, EventArgs e)
        {
            if (_selectedStudentId == 0) { NotificationHelper.ShowWarning("Please search and select a student first."); return; }
            if (!NotificationHelper.Confirm($"Mark '{_selectedStudentName}' as Absent?")) return;
            var (ok, msg) = _service.MarkAbsent(_selectedStudentId, _selectedStudentName);
            ShowStatus(msg, ok);
            if (ok) LoadTodayAttendance();
        }

        private void btnRefresh_Click(object sender, EventArgs e) => LoadTodayAttendance();

        private void ShowStatus(string msg, bool success) =>
            NotificationHelper.ShowBanner(lblStatus, msg,
                success ? NotificationType.Success : NotificationType.Error);
    }
}

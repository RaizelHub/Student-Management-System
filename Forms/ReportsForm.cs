using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using StudentAttendanceSysttem.Database;
using StudentAttendanceSysttem.Repositories;
using StudentAttendanceSysttem.Services;
using StudentAttendanceSysttem.Utilities;

namespace StudentAttendanceSysttem.Forms
{
    public partial class ReportsForm : Form
    {
        private readonly ReportService _reportService;
        private readonly DatabaseConnection _db;
        private DataTable? _currentData;

        public ReportsForm()
        {
            InitializeComponent();
            _db            = new DatabaseConnection();
            _reportService = new ReportService(new AttendanceRepository(_db));
        }

        private void ReportsForm_Load(object sender, EventArgs e)
        {
            ThemeManager.ApplyTheme(this);
            ThemeManager.StyleDataGridView(dgvReport);
            ThemeManager.StylePrimaryButton(btnGenerate);
            ThemeManager.StyleSecondaryButton(btnExportPdf);
            ThemeManager.StyleSecondaryButton(btnExportExcel);
            btnExportPdf.BackColor   = ThemeManager.Danger;
            btnExportExcel.BackColor = ThemeManager.Success;
            dtpFrom.Value = DateTime.Today;
            dtpTo.Value   = DateTime.Today;
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                string type = cmbReportType.Text;
                _currentData = type switch
                {
                    "Daily Attendance"   => _reportService.GetDailyReport(dtpFrom.Value),
                    "Weekly Attendance"  => _reportService.GetRangeReport(DateTime.Today.AddDays(-7), DateTime.Today),
                    "Monthly Attendance" => _reportService.GetRangeReport(new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1), DateTime.Today),
                    "Date Range"         => _reportService.GetRangeReport(dtpFrom.Value, dtpTo.Value),
                    _                    => _reportService.GetDailyReport(DateTime.Today)
                };

                dgvReport.DataSource = _currentData;
                lblCount.Text        = $"Records: {_currentData.Rows.Count}";
            }
            catch (Exception ex) { NotificationHelper.ShowError(ex.Message); }
        }

        private void btnExportPdf_Click(object sender, EventArgs e)
        {
            if (_currentData == null) { NotificationHelper.ShowWarning("Generate a report first."); return; }
            var (ok, path) = _reportService.ExportToPdf(_currentData, cmbReportType.Text);
            if (ok) { NotificationHelper.ShowSuccess($"PDF saved to:\n{path}"); OpenFile(path); }
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            if (_currentData == null) { NotificationHelper.ShowWarning("Generate a report first."); return; }
            var (ok, path) = _reportService.ExportToExcel(_currentData, cmbReportType.Text);
            if (ok) { NotificationHelper.ShowSuccess($"Excel saved to:\n{path}"); OpenFile(path); }
        }

        private static void OpenFile(string path)
        {
            try { Process.Start(new ProcessStartInfo(path) { UseShellExecute = true }); }
            catch { }
        }
    }
}

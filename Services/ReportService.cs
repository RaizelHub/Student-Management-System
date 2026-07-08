using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using ClosedXML.Excel;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using StudentAttendanceSysttem.Repositories;
using StudentAttendanceSysttem.Utilities;

namespace StudentAttendanceSysttem.Services
{
    /// <summary>
    /// Generates PDF and Excel attendance reports.
    /// Uses iText7 for PDF and ClosedXML for Excel.
    /// </summary>
    public class ReportService
    {
        private readonly AttendanceRepository _repo;

        public ReportService(AttendanceRepository repo) => _repo = repo;

        // ─── PDF ──────────────────────────────────────────────────────────────────

        public string GeneratePdf(DataTable data, string reportTitle, string outputPath)
        {
            var boldFont   = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            var regularFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

            using var writer   = new PdfWriter(outputPath);
            using var pdf      = new PdfDocument(writer);
            using var document = new Document(pdf);

            // Title
            document.Add(new Paragraph(reportTitle)
                .SetFont(boldFont).SetFontSize(18).SetMarginBottom(4));

            document.Add(new Paragraph($"Generated: {DateTime.Now:MMMM dd, yyyy hh:mm tt}  |  By: {SessionManager.Instance.FullName}")
                .SetFont(regularFont).SetFontSize(9).SetMarginBottom(12));

            // Table
            var table = new Table(data.Columns.Count)
                .SetWidth(UnitValue.CreatePercentValue(100));

            // Header
            foreach (DataColumn col in data.Columns)
                table.AddHeaderCell(new Cell().Add(
                    new Paragraph(col.ColumnName).SetFont(boldFont).SetFontSize(9)));

            // Rows
            foreach (DataRow row in data.Rows)
                foreach (var item in row.ItemArray)
                    table.AddCell(new Cell().Add(
                        new Paragraph(item?.ToString() ?? "").SetFont(regularFont).SetFontSize(8)));

            document.Add(table);
            return outputPath;
        }

        // ─── Excel ────────────────────────────────────────────────────────────────

        public string GenerateExcel(DataTable data, string sheetName, string outputPath)
        {
            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add(sheetName.Length > 31 ? sheetName[..31] : sheetName);

            // Header row
            for (int c = 0; c < data.Columns.Count; c++)
            {
                var cell = ws.Cell(1, c + 1);
                cell.Value                      = data.Columns[c].ColumnName;
                cell.Style.Font.Bold            = true;
                cell.Style.Fill.BackgroundColor = XLColor.FromArgb(99, 102, 241);
                cell.Style.Font.FontColor       = XLColor.White;
            }

            // Data rows
            for (int r = 0; r < data.Rows.Count; r++)
                for (int c = 0; c < data.Columns.Count; c++)
                    ws.Cell(r + 2, c + 1).Value = data.Rows[r][c]?.ToString() ?? "";

            ws.Columns().AdjustToContents();
            ws.SheetView.Freeze(1, 0);

            wb.SaveAs(outputPath);
            return outputPath;
        }

        // ─── Convenience ──────────────────────────────────────────────────────────

        public DataTable GetDailyReport(DateTime date) => _repo.GetByDate(date);

        public DataTable GetRangeReport(DateTime from, DateTime to, int? courseId = null, int? sectionId = null) =>
            _repo.GetByDateRange(from, to, courseId, sectionId);

        public DataTable GetStudentHistory(int studentId) =>
            _repo.GetStudentHistory(studentId);

        public (bool Success, string Path) ExportToPdf(DataTable data, string title)
        {
            string path = GetSavePath($"{title}_{DateTime.Today:yyyyMMdd}.pdf",
                "PDF Files|*.pdf", "pdf");
            if (string.IsNullOrEmpty(path)) return (false, "");
            GeneratePdf(data, title, path);
            return (true, path);
        }

        public (bool Success, string Path) ExportToExcel(DataTable data, string sheetName)
        {
            string path = GetSavePath($"{sheetName}_{DateTime.Today:yyyyMMdd}.xlsx",
                "Excel Files|*.xlsx", "xlsx");
            if (string.IsNullOrEmpty(path)) return (false, "");
            GenerateExcel(data, sheetName, path);
            return (true, path);
        }

        private static string GetSavePath(string defaultName, string filter, string ext)
        {
            using var dlg = new SaveFileDialog
            {
                FileName         = defaultName,
                Filter           = filter,
                DefaultExt       = ext,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };
            return dlg.ShowDialog() == DialogResult.OK ? dlg.FileName : string.Empty;
        }
    }
}

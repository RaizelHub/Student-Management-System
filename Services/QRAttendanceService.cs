using System;
using System.Drawing;
using StudentAttendanceSysttem.Database;
using StudentAttendanceSysttem.Repositories;
using StudentAttendanceSysttem.Services;
using StudentAttendanceSysttem.Utilities;
using ZXing;
using ZXing.Windows.Compatibility;

namespace StudentAttendanceSysttem.Services
{
    /// <summary>
    /// Handles QR code scanning for attendance recording.
    /// Decodes QR bitmaps using ZXing.Net and records attendance via AttendanceService.
    /// </summary>
    public class QRAttendanceService
    {
        private readonly AttendanceRepository _attendanceRepo;
        private readonly StudentRepository    _studentRepo;
        private readonly AuditService         _audit;

        public QRAttendanceService(
            AttendanceRepository attendanceRepo,
            StudentRepository    studentRepo,
            AuditService         audit)
        {
            _attendanceRepo = attendanceRepo;
            _studentRepo    = studentRepo;
            _audit          = audit;
        }

        /// <summary>
        /// Decodes a QR code from a <see cref="Bitmap"/> and returns the encoded text.
        /// Returns null if decoding fails.
        /// </summary>
        public string? DecodeQRCode(Bitmap image)
        {
            var reader = new BarcodeReader
            {
                AutoRotate          = true,
                Options = { TryHarder = true, TryInverted = true }
            };

            var result = reader.Decode(image);
            return result?.Text;
        }

        /// <summary>
        /// Decodes the QR code and records attendance (Time In or Time Out).
        /// The QR code must encode the student's <c>student_number</c>.
        /// </summary>
        public (bool Success, string Message) ProcessQRAttendance(Bitmap qrImage)
        {
            string? studentNumber = DecodeQRCode(qrImage);
            if (string.IsNullOrEmpty(studentNumber))
                return (false, "Could not decode QR code. Please try again.");

            var student = _studentRepo.GetAll().Find(s =>
                s.StudentNumber.Equals(studentNumber, StringComparison.OrdinalIgnoreCase));

            if (student == null)
                return (false, $"No student found with QR code: {studentNumber}");

            // If already has time-in but no time-out → time out
            var todayRecord = _attendanceRepo.GetTodayRecord(student.Id);
            if (todayRecord != null && !todayRecord.TimeOut.HasValue)
            {
                _attendanceRepo.TimeOut(todayRecord.Id);
                _audit.LogAction("QR_TIME_OUT", $"QR Time Out: {student.FullName}");
                return (true, $"✅ Time Out recorded for {student.FullName}");
            }

            // Otherwise → time in
            if (_attendanceRepo.HasAttendanceToday(student.Id))
                return (false, $"{student.FullName} already has a complete attendance record today.");

            string status = DateTime.Now.TimeOfDay > new TimeSpan(8, 30, 0) ? "Late" : "Present";
            _attendanceRepo.TimeIn(student.Id, status);
            _audit.LogAction("QR_TIME_IN", $"QR Time In: {student.FullName} — {status}");
            return (true, $"✅ Time In recorded for {student.FullName} — {status}");
        }
    }
}

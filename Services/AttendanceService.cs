using System;
using System.Data;
using StudentAttendanceSysttem.Models;
using StudentAttendanceSysttem.Repositories;

namespace StudentAttendanceSysttem.Services
{
    /// <summary>
    /// Business logic for recording student attendance.
    /// Enforces duplicate prevention and late-arrival logic.
    /// Late threshold: students arriving after 08:30 AM are marked Late.
    /// </summary>
    public class AttendanceService
    {
        private readonly AttendanceRepository _repo;
        private readonly AuditService         _audit;

        private static readonly TimeSpan LateThreshold = new TimeSpan(8, 30, 0);

        public AttendanceService(AttendanceRepository repo, AuditService audit)
        { _repo = repo; _audit = audit; }

        public (bool Success, string Message, int RecordId) TimeIn(int studentId, string studentName)
        {
            if (_repo.HasAttendanceToday(studentId))
                return (false, $"{studentName} already has an attendance record today.", 0);

            string status = DateTime.Now.TimeOfDay > LateThreshold ? "Late" : "Present";
            int id = _repo.TimeIn(studentId, status);
            _audit.LogAction("TIME_IN", $"Time In: {studentName} — Status: {status}");
            return (true, $"Time In recorded. Status: {status}", id);
        }

        public (bool Success, string Message) TimeOut(int studentId, string studentName)
        {
            var record = _repo.GetTodayRecord(studentId);
            if (record == null)
                return (false, $"No Time In record found for {studentName} today.");
            if (record.TimeOut.HasValue)
                return (false, $"{studentName} already has Time Out recorded today.");

            bool ok = _repo.TimeOut(record.Id);
            if (ok) _audit.LogAction("TIME_OUT", $"Time Out: {studentName}");
            return (ok, ok ? "Time Out recorded." : "Failed to record Time Out.");
        }

        public (bool Success, string Message) MarkAbsent(int studentId, string studentName)
        {
            if (_repo.HasAttendanceToday(studentId))
                return (false, $"{studentName} already has an attendance record today.");

            bool ok = _repo.MarkAbsent(studentId);
            if (ok) _audit.LogAction("MARK_ABSENT", $"Marked absent: {studentName}");
            return (ok, ok ? $"{studentName} marked as Absent." : "Failed to mark absent.");
        }

        public DataTable GetTodayAttendance()         => _repo.GetByDate(DateTime.Today);
        public DataTable GetAttendanceByDate(DateTime d) => _repo.GetByDate(d);

        public DataTable GetByDateRange(DateTime from, DateTime to, int? courseId = null, int? sectionId = null) =>
            _repo.GetByDateRange(from, to, courseId, sectionId);

        public DataTable GetStudentHistory(int studentId) =>
            _repo.GetStudentHistory(studentId);
    }
}

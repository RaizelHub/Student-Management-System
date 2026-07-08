using StudentAttendanceSysttem.Models;
using System.Drawing;

namespace StudentAttendanceSysttem.Helpers
{
    /// <summary>
    /// Application-level fingerprint service interface.
    /// Bridges hardware reader to student identity and attendance flow.
    /// </summary>
    public interface IFingerprintService
    {
        /// <summary>Enroll a student's fingerprint and store the template ID in their record.</summary>
        (bool Success, string Message) EnrollStudent(int studentId);

        /// <summary>Record attendance for the student whose fingerprint matches the scanned finger.</summary>
        (bool Success, string Message) RecordAttendanceByFingerprint();

        /// <summary>Remove a student's enrolled fingerprint template.</summary>
        bool RemoveEnrollment(int studentId);

        /// <summary>Returns true if the given student has an enrolled fingerprint.</summary>
        bool IsEnrolled(int studentId);
    }
}

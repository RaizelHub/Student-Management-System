using StudentAttendanceSysttem.Models;

namespace StudentAttendanceSysttem.Helpers
{
    /// <summary>
    /// Application-level RFID service interface.
    /// Bridges IRfidReader hardware to student lookup and attendance recording.
    /// </summary>
    public interface IRfidService
    {
        /// <summary>Looks up a student by their registered RFID card UID.</summary>
        Student? FindStudentByRfid(string cardUid);

        /// <summary>Records Time In for the student identified by the given RFID card UID.</summary>
        (bool Success, string Message) ProcessTimeIn(string cardUid);

        /// <summary>Records Time Out for the student identified by the given RFID card UID.</summary>
        (bool Success, string Message) ProcessTimeOut(string cardUid);

        /// <summary>Registers an RFID card UID to a student record.</summary>
        bool RegisterCard(int studentId, string cardUid);

        /// <summary>Removes the RFID card association from a student record.</summary>
        bool UnregisterCard(int studentId);
    }
}

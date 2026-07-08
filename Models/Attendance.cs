using System;

namespace StudentAttendanceSysttem.Models
{
    /// <summary>Maps to the <c>attendance</c> table.</summary>
    public class Attendance
    {
        public int      Id          { get; set; }
        public int      StudentId   { get; set; }
        public string   StudentName { get; set; } = string.Empty;
        public string   StudentNumber { get; set; } = string.Empty;
        public string   CourseName  { get; set; } = string.Empty;
        public string   SectionName { get; set; } = string.Empty;
        public DateTime Date        { get; set; } = DateTime.Today;
        public TimeSpan? TimeIn     { get; set; }
        public TimeSpan? TimeOut    { get; set; }
        public string   Status      { get; set; } = "Present"; // Present | Absent | Late
        public string   Remarks     { get; set; } = string.Empty;
        public DateTime CreatedAt   { get; set; } = DateTime.Now;
    }
}

using System;

namespace StudentAttendanceSysttem.Models
{
    /// <summary>Maps to the <c>courses</c> table.</summary>
    public class Course
    {
        public int      Id          { get; set; }
        public string   CourseCode  { get; set; } = string.Empty;
        public string   CourseName  { get; set; } = string.Empty;
        public string   Description { get; set; } = string.Empty;
        public bool     IsActive    { get; set; } = true;
        public DateTime CreatedAt   { get; set; } = DateTime.Now;
        public DateTime UpdatedAt   { get; set; } = DateTime.Now;

        public override string ToString() => CourseName;
    }
}

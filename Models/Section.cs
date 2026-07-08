using System;

namespace StudentAttendanceSysttem.Models
{
    /// <summary>Maps to the <c>sections</c> table.</summary>
    public class Section
    {
        public int      Id          { get; set; }
        public string   SectionName { get; set; } = string.Empty;
        public int      CourseId    { get; set; }
        public string   CourseName  { get; set; } = string.Empty;
        public string   YearLevel   { get; set; } = string.Empty;
        public string   SchoolYear  { get; set; } = string.Empty;
        public bool     IsActive    { get; set; } = true;
        public DateTime CreatedAt   { get; set; } = DateTime.Now;
        public DateTime UpdatedAt   { get; set; } = DateTime.Now;

        public override string ToString() => SectionName;
    }
}

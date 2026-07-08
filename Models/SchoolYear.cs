using System;

namespace StudentAttendanceSysttem.Models
{
    /// <summary>Maps to the <c>school_year</c> table.</summary>
    public class SchoolYear
    {
        public int      Id          { get; set; }
        public string   YearLabel   { get; set; } = string.Empty; // e.g. "2024-2025"
        public string   Semester    { get; set; } = string.Empty; // e.g. "1st Semester"
        public DateTime StartDate   { get; set; }
        public DateTime EndDate     { get; set; }
        public bool     IsCurrent   { get; set; } = false;

        public override string ToString() => $"{YearLabel} — {Semester}";
    }
}

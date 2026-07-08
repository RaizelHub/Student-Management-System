using System;

namespace StudentAttendanceSysttem.Models
{
    /// <summary>
    /// Represents a student record. Maps to the <c>students</c> table.
    /// </summary>
    public class Student
    {
        public int      Id              { get; set; }
        public string   StudentNumber   { get; set; } = string.Empty;
        public string   FirstName       { get; set; } = string.Empty;
        public string   MiddleName      { get; set; } = string.Empty;
        public string   LastName        { get; set; } = string.Empty;
        public string   Gender          { get; set; } = string.Empty;
        public DateTime? BirthDate      { get; set; }
        public string   Address         { get; set; } = string.Empty;
        public string   ContactNumber   { get; set; } = string.Empty;
        public string   Email           { get; set; } = string.Empty;
        public int      CourseId        { get; set; }
        public string   CourseName      { get; set; } = string.Empty;
        public int      SectionId       { get; set; }
        public string   SectionName     { get; set; } = string.Empty;
        public string   YearLevel       { get; set; } = string.Empty;
        public string   RfidNumber      { get; set; } = string.Empty;
        public string   FingerprintId   { get; set; } = string.Empty;
        public string   QrCode          { get; set; } = string.Empty;
        public string?  PhotoPath       { get; set; }
        public bool     IsActive        { get; set; } = true;
        public DateTime CreatedAt       { get; set; } = DateTime.Now;
        public DateTime UpdatedAt       { get; set; } = DateTime.Now;

        // ── Computed ──────────────────────────────────────────────────────────────
        public string FullName =>
            string.IsNullOrWhiteSpace(MiddleName)
                ? $"{FirstName} {LastName}".Trim()
                : $"{FirstName} {MiddleName[0]}. {LastName}".Trim();
    }
}

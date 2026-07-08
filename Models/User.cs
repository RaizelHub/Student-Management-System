using System;

namespace StudentAttendanceSysttem.Models
{
    /// <summary>
    /// Represents an application user. Maps to the <c>users</c> table.
    /// Actual DB columns: id, username, password, role, full_name,
    ///                    is_active, failed_login_attempts, last_login, created_at
    /// </summary>
    public class User
    {
        public int       Id                   { get; set; }
        public string    Username             { get; set; } = string.Empty;

        /// <summary>BCrypt hash stored in DB column <c>password</c>. Never store plain text.</summary>
        public string    PasswordHash         { get; set; } = string.Empty;

        /// <summary>Maps to DB column <c>full_name</c>.</summary>
        public string    FullName             { get; set; } = string.Empty;

        // FirstName / LastName are derived from FullName for compatibility with other layers
        public string    FirstName
        {
            get
            {
                var parts = FullName.Trim().Split(' ');
                return parts.Length > 0 ? parts[0] : FullName;
            }
            set => FullName = string.IsNullOrWhiteSpace(LastName)
                    ? value.Trim()
                    : $"{value.Trim()} {LastName.Trim()}";
        }

        public string    LastName
        {
            get
            {
                var parts = FullName.Trim().Split(' ');
                return parts.Length > 1 ? string.Join(" ", parts[1..]) : "";
            }
            set
            {
                var parts = FullName.Trim().Split(' ');
                string fn  = parts.Length > 0 ? parts[0] : "";
                FullName   = string.IsNullOrWhiteSpace(value)
                    ? fn : $"{fn} {value.Trim()}";
            }
        }

        public string    Email                { get; set; } = string.Empty;
        public string    Role                 { get; set; } = "Staff"; // "Administrator" | "Staff"
        public bool      IsActive             { get; set; } = true;
        public int       FailedAttempts       { get; set; } = 0;
        public DateTime? LockedUntil          { get; set; }
        public DateTime? LastLogin            { get; set; }
        public DateTime  CreatedAt            { get; set; } = DateTime.Now;

        public bool IsLocked =>
            LockedUntil.HasValue && LockedUntil.Value > DateTime.Now;
    }
}

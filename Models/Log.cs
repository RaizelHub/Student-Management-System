using System;

namespace StudentAttendanceSysttem.Models
{
    /// <summary>Maps to the <c>logs</c> table.</summary>
    public class Log
    {
        public int      Id          { get; set; }
        public int      UserId      { get; set; }
        public string   Username    { get; set; } = string.Empty;
        public string   Action      { get; set; } = string.Empty;
        public string   Description { get; set; } = string.Empty;
        public string   IpAddress   { get; set; } = string.Empty;
        public DateTime CreatedAt   { get; set; } = DateTime.Now;
    }
}

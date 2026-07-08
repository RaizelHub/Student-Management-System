using System;
using System.Configuration;

namespace StudentAttendanceSysttem.Utilities
{
    /// <summary>
    /// Singleton session manager. Holds the currently authenticated user and
    /// enforces configurable session expiry from App.config (SessionTimeoutHours).
    /// </summary>
    public sealed class SessionManager
    {
        // ─── Singleton ────────────────────────────────────────────────────────────
        private static readonly Lazy<SessionManager> _instance =
            new(() => new SessionManager());

        public static SessionManager Instance => _instance.Value;

        private SessionManager() { }

        // ─── Session Data ─────────────────────────────────────────────────────────
        public int    UserId        { get; private set; }
        public string Username      { get; private set; } = string.Empty;
        public string FullName      { get; private set; } = string.Empty;
        public string Role          { get; private set; } = string.Empty;
        public DateTime LoginTime   { get; private set; }
        public bool   IsLoggedIn    { get; private set; }

        private static readonly int TimeoutHours =
            int.TryParse(ConfigurationManager.AppSettings["SessionTimeoutHours"], out int h) ? h : 8;

        // ─── Public API ───────────────────────────────────────────────────────────

        /// <summary>Starts a new authenticated session for the given user.</summary>
        public void Login(int userId, string username, string fullName, string role)
        {
            UserId    = userId;
            Username  = username;
            FullName  = fullName;
            Role      = role;
            LoginTime = DateTime.Now;
            IsLoggedIn = true;
        }

        /// <summary>Clears the current session.</summary>
        public void Logout()
        {
            UserId    = 0;
            Username  = string.Empty;
            FullName  = string.Empty;
            Role      = string.Empty;
            IsLoggedIn = false;
        }

        /// <summary>Returns true if the session has exceeded the configured timeout.</summary>
        public bool IsSessionExpired()
        {
            if (!IsLoggedIn) return true;
            return DateTime.Now - LoginTime > TimeSpan.FromHours(TimeoutHours);
        }

        /// <summary>Returns true if the current user has the specified role (case-insensitive).</summary>
        public bool HasRole(string role) =>
            IsLoggedIn && string.Equals(Role, role, StringComparison.OrdinalIgnoreCase);

        /// <summary>Returns true if the current user is an Administrator.</summary>
        public bool IsAdmin => HasRole("Administrator");

        /// <summary>Returns true if the current user is Staff or Admin.</summary>
        public bool IsStaff => HasRole("Staff") || IsAdmin;
    }
}

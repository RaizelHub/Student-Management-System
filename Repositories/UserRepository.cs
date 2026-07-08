using System;
using System.Collections.Generic;
using MySqlConnector;
using StudentAttendanceSysttem.Database;
using StudentAttendanceSysttem.Models;

namespace StudentAttendanceSysttem.Repositories
{
    /// <summary>
    /// Data access layer for the <c>users</c> table.
    /// Actual DB columns: id, username, password, role, full_name,
    ///                    is_active, failed_login_attempts, last_login, created_at
    /// </summary>
    public class UserRepository
    {
        private readonly DatabaseConnection _db;

        public UserRepository(DatabaseConnection db) => _db = db;

        // ─── Read ─────────────────────────────────────────────────────────────────

        public User? GetByUsername(string username)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            const string sql = @"
                SELECT id, username, password, role, full_name,
                       is_active, failed_login_attempts, last_login, created_at
                FROM   users
                WHERE  username = @username
                LIMIT  1";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@username", username);
            using var reader = cmd.ExecuteReader();
            return reader.Read() ? MapUser(reader) : null;
        }

        public User? GetById(int id)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            const string sql = @"
                SELECT id, username, password, role, full_name,
                       is_active, failed_login_attempts, last_login, created_at
                FROM   users WHERE id = @id LIMIT 1";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            using var reader = cmd.ExecuteReader();
            return reader.Read() ? MapUser(reader) : null;
        }

        public List<User> GetAll()
        {
            var users = new List<User>();
            using var conn = _db.GetConnection();
            conn.Open();
            const string sql = @"
                SELECT id, username, password, role, full_name,
                       is_active, failed_login_attempts, last_login, created_at
                FROM   users ORDER BY full_name";

            using var cmd    = new MySqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read()) users.Add(MapUser(reader));
            return users;
        }

        // ─── Write ────────────────────────────────────────────────────────────────

        public int Add(User user)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            const string sql = @"
                INSERT INTO users (username, password, role, full_name, is_active, created_at)
                VALUES (@username, @password, @role, @fullName, @isActive, NOW());
                SELECT LAST_INSERT_ID();";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@username", user.Username);
            cmd.Parameters.AddWithValue("@password", user.PasswordHash);
            cmd.Parameters.AddWithValue("@role",     user.Role);
            cmd.Parameters.AddWithValue("@fullName", user.FullName);
            cmd.Parameters.AddWithValue("@isActive", user.IsActive ? 1 : 0);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public bool Update(User user)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            const string sql = @"
                UPDATE users SET
                    full_name = @fullName, role = @role, is_active = @isActive
                WHERE id = @id";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@fullName", user.FullName);
            cmd.Parameters.AddWithValue("@role",     user.Role);
            cmd.Parameters.AddWithValue("@isActive", user.IsActive ? 1 : 0);
            cmd.Parameters.AddWithValue("@id",       user.Id);
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool UpdatePassword(int userId, string newHash)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            const string sql = "UPDATE users SET password = @hash WHERE id = @id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@hash", newHash);
            cmd.Parameters.AddWithValue("@id",   userId);
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Delete(int id)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            const string sql = "DELETE FROM users WHERE id = @id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            return cmd.ExecuteNonQuery() > 0;
        }

        // ─── Lockout / Auth Helpers ───────────────────────────────────────────────

        public void IncrementFailedAttempts(int userId)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            const string sql = "UPDATE users SET failed_login_attempts = failed_login_attempts + 1 WHERE id = @id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", userId);
            cmd.ExecuteNonQuery();
        }

        public void LockAccount(int userId, int lockoutMinutes)
        {
            // Store lock expiry in memory only — DB has no locked_until column
            // The in-memory lockout is enforced by UserService via FailedAttempts threshold
            IncrementFailedAttempts(userId);
        }

        public void ResetFailedAttempts(int userId)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            const string sql = "UPDATE users SET failed_login_attempts = 0 WHERE id = @id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", userId);
            cmd.ExecuteNonQuery();
        }

        public void UpdateLastLogin(int userId)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            const string sql = "UPDATE users SET last_login = NOW() WHERE id = @id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", userId);
            cmd.ExecuteNonQuery();
        }

        public bool UsernameExists(string username, int excludeId = 0)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            const string sql = "SELECT COUNT(*) FROM users WHERE username = @username AND id != @excludeId";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@username",  username);
            cmd.Parameters.AddWithValue("@excludeId", excludeId);
            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }

        // ─── Mapping ──────────────────────────────────────────────────────────────

        private static User MapUser(MySqlDataReader r) => new()
        {
            Id             = r.GetInt32("id"),
            Username       = r.GetString("username"),
            PasswordHash   = r.GetString("password"),
            FullName       = r.IsDBNull(r.GetOrdinal("full_name")) ? "" : r.GetString("full_name"),
            Role           = r.GetString("role"),
            IsActive       = r.GetBoolean("is_active"),
            FailedAttempts = r.IsDBNull(r.GetOrdinal("failed_login_attempts")) ? 0 : r.GetInt32("failed_login_attempts"),
            LockedUntil    = null,   // no locked_until column in DB
            LastLogin      = r.IsDBNull(r.GetOrdinal("last_login")) ? null : r.GetDateTime("last_login"),
            CreatedAt      = r.IsDBNull(r.GetOrdinal("created_at")) ? DateTime.Now : r.GetDateTime("created_at"),
        };
    }
}

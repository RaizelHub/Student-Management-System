using System;
using System.Configuration;
using StudentAttendanceSysttem.Models;
using StudentAttendanceSysttem.Repositories;
using StudentAttendanceSysttem.Utilities;

namespace StudentAttendanceSysttem.Services
{
    /// <summary>
    /// Business logic for user authentication and management.
    /// Supports both plain-text passwords (legacy) and BCrypt hashes.
    /// On first login with a plain-text password, the hash is automatically
    /// upgraded to BCrypt and saved — no manual migration needed.
    /// </summary>
    public class UserService
    {
        private readonly UserRepository _repo;

        private static readonly int LockoutThreshold =
            int.TryParse(ConfigurationManager.AppSettings["AccountLockoutThreshold"], out int t) ? t : 5;

        private static readonly int LockoutMinutes =
            int.TryParse(ConfigurationManager.AppSettings["AccountLockoutMinutes"], out int m) ? m : 15;

        public UserService(UserRepository repo) => _repo = repo;

        // ─── Authentication ───────────────────────────────────────────────────────

        public LoginResult Authenticate(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return LoginResult.Failure("Username and password are required.");

            var user = _repo.GetByUsername(username.Trim());
            if (user == null)
                return LoginResult.Failure("Invalid username or password.");

            if (!user.IsActive)
                return LoginResult.Failure("Your account has been deactivated. Contact an administrator.");

            // ── Lockout check (count-based, no locked_until column) ───────────────
            if (user.FailedAttempts >= LockoutThreshold)
            {
                return LoginResult.Failure(
                    $"Account is temporarily locked after {LockoutThreshold} failed attempts.\n" +
                    "Please contact an administrator to unlock your account.");
            }

            // ── Password verification ─────────────────────────────────────────────
            bool passwordOk = false;

            if (IsBCryptHash(user.PasswordHash))
            {
                // Modern BCrypt path
                passwordOk = PasswordHelper.VerifyPassword(password, user.PasswordHash);
            }
            else
            {
                // Legacy plain-text path — compare directly
                passwordOk = user.PasswordHash == password;

                if (passwordOk)
                {
                    // AUTO-UPGRADE: replace plain text with BCrypt hash on first successful login
                    try
                    {
                        string newHash = PasswordHelper.HashPassword(password);
                        _repo.UpdatePassword(user.Id, newHash);
                        System.Diagnostics.Debug.WriteLine(
                            $"[UserService] Password auto-upgraded to BCrypt for user: {username}");
                    }
                    catch (Exception ex)
                    {
                        // Upgrade failure is non-fatal — login still succeeds
                        System.Diagnostics.Debug.WriteLine(
                            $"[UserService] BCrypt upgrade failed: {ex.Message}");
                    }
                }
            }

            if (!passwordOk)
            {
                _repo.IncrementFailedAttempts(user.Id);

                int attemptsLeft = LockoutThreshold - (user.FailedAttempts + 1);
                if (attemptsLeft <= 0)
                {
                    _repo.LockAccount(user.Id, LockoutMinutes);
                    return LoginResult.Failure(
                        $"Too many failed attempts. Account is now locked.\n" +
                        "Please contact an administrator to unlock your account.");
                }

                return LoginResult.Failure(
                    $"Invalid username or password. {attemptsLeft} attempt(s) remaining.");
            }

            // ── Success ───────────────────────────────────────────────────────────
            _repo.ResetFailedAttempts(user.Id);
            _repo.UpdateLastLogin(user.Id);

            SessionManager.Instance.Login(user.Id, user.Username, user.FullName, user.Role);

            return LoginResult.Success(user);
        }

        /// <summary>Returns true if the string looks like a BCrypt hash ($2a$, $2b$, $2y$).</summary>
        private static bool IsBCryptHash(string hash) =>
            !string.IsNullOrEmpty(hash) &&
            hash.Length >= 7 &&
            hash[0] == '$' && hash[1] == '2';

        public void Logout() => SessionManager.Instance.Logout();

        // ─── Password Management ──────────────────────────────────────────────────

        public bool ChangePassword(int userId, string currentPassword, string newPassword)
        {
            var user = _repo.GetById(userId);
            if (user == null) return false;

            bool valid = IsBCryptHash(user.PasswordHash)
                ? PasswordHelper.VerifyPassword(currentPassword, user.PasswordHash)
                : user.PasswordHash == currentPassword;

            if (!valid) return false;

            return _repo.UpdatePassword(userId, PasswordHelper.HashPassword(newPassword));
        }

        public string ResetPassword(int userId)
        {
            var temp = PasswordHelper.GenerateTemporaryPassword();
            _repo.UpdatePassword(userId, PasswordHelper.HashPassword(temp));
            return temp;
        }

        // ─── CRUD ─────────────────────────────────────────────────────────────────

        public int CreateUser(User user, string plainPassword)
        {
            user.PasswordHash = PasswordHelper.HashPassword(plainPassword);
            return _repo.Add(user);
        }

        public bool UpdateUser(User user) => _repo.Update(user);

        public bool DeleteUser(int id) => _repo.Delete(id);

        public System.Collections.Generic.List<User> GetAllUsers() => _repo.GetAll();

        public bool UsernameExists(string username, int excludeId = 0) =>
            _repo.UsernameExists(username, excludeId);
    }

    // ─── Result Type ──────────────────────────────────────────────────────────────

    public sealed class LoginResult
    {
        public bool   IsSuccess { get; private init; }
        public string Message   { get; private init; } = string.Empty;
        public User?  User      { get; private init; }

        public static LoginResult Success(User user) =>
            new() { IsSuccess = true, Message = "Login successful.", User = user };

        public static LoginResult Failure(string message) =>
            new() { IsSuccess = false, Message = message };
    }
}

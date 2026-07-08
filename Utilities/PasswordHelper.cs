using BCrypt.Net;

namespace StudentAttendanceSysttem.Utilities
{
    /// <summary>
    /// Provides BCrypt password hashing and verification.
    /// Work factor 12 is a good production balance between security and performance (~300ms on modern hardware).
    /// </summary>
    public static class PasswordHelper
    {
        private const int WorkFactor = 12;

        /// <summary>
        /// Hashes a plain-text password using BCrypt.
        /// Store the returned hash in the database; never store the plain text.
        /// </summary>
        public static string HashPassword(string plainTextPassword) =>
            BCrypt.Net.BCrypt.HashPassword(plainTextPassword, WorkFactor);

        /// <summary>
        /// Verifies a plain-text password against a stored BCrypt hash.
        /// Returns true if they match, false otherwise.
        /// </summary>
        public static bool VerifyPassword(string plainTextPassword, string hashedPassword)
        {
            if (string.IsNullOrWhiteSpace(plainTextPassword) ||
                string.IsNullOrWhiteSpace(hashedPassword))
                return false;

            try
            {
                return BCrypt.Net.BCrypt.Verify(plainTextPassword, hashedPassword);
            }
            catch
            {
                // Malformed hash — treat as mismatch
                return false;
            }
        }

        /// <summary>
        /// Generates a cryptographically secure random temporary password (12 chars).
        /// Useful for password reset flows.
        /// </summary>
        public static string GenerateTemporaryPassword()
        {
            const string chars = "ABCDEFGHJKMNPQRSTUVWXYZabcdefghjkmnpqrstuvwxyz23456789!@#$";
            var bytes = new byte[12];
            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            rng.GetBytes(bytes);

            var result = new char[12];
            for (int i = 0; i < 12; i++)
                result[i] = chars[bytes[i] % chars.Length];

            return new string(result);
        }
    }
}

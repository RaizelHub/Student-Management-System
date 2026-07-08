using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace StudentAttendanceSysttem.Utilities
{
    /// <summary>
    /// Encapsulates all validation rules for user input.
    /// All methods return a <see cref="ValidationResult"/> containing any error messages.
    /// </summary>
    public static class ValidationHelper
    {
        // ─── Primitive Checks ─────────────────────────────────────────────────────

        public static ValidationResult IsNotEmpty(string? value, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value))
                return ValidationResult.Fail($"{fieldName} is required.");
            return ValidationResult.Ok();
        }

        public static ValidationResult IsValidEmail(string? value, string fieldName = "Email")
        {
            if (string.IsNullOrWhiteSpace(value))
                return ValidationResult.Fail($"{fieldName} is required.");

            var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(value, pattern))
                return ValidationResult.Fail($"{fieldName} is not a valid email address.");

            return ValidationResult.Ok();
        }

        public static ValidationResult IsValidPhoneNumber(string? value, string fieldName = "Contact Number")
        {
            if (string.IsNullOrWhiteSpace(value))
                return ValidationResult.Ok(); // Phone is optional unless caller checks IsNotEmpty first

            var pattern = @"^[\d\+\-\(\)\s]{7,15}$";
            if (!Regex.IsMatch(value, pattern))
                return ValidationResult.Fail($"{fieldName} must be a valid phone number (7-15 digits).");

            return ValidationResult.Ok();
        }

        public static ValidationResult IsValidStudentNumber(string? value, string fieldName = "Student Number")
        {
            if (string.IsNullOrWhiteSpace(value))
                return ValidationResult.Fail($"{fieldName} is required.");

            // Format: digits, dashes and letters allowed — e.g. 2024-00001
            var pattern = @"^[A-Za-z0-9\-]{3,20}$";
            if (!Regex.IsMatch(value, pattern))
                return ValidationResult.Fail($"{fieldName} must be 3-20 alphanumeric characters (dashes allowed).");

            return ValidationResult.Ok();
        }

        public static ValidationResult IsMaxLength(string? value, int max, string fieldName)
        {
            if (value != null && value.Length > max)
                return ValidationResult.Fail($"{fieldName} must not exceed {max} characters.");
            return ValidationResult.Ok();
        }

        public static ValidationResult IsValidDate(string? value, string fieldName = "Date")
        {
            if (!DateTime.TryParse(value, out _))
                return ValidationResult.Fail($"{fieldName} is not a valid date.");
            return ValidationResult.Ok();
        }

        public static ValidationResult IsStrongPassword(string? value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length < 8)
                return ValidationResult.Fail("Password must be at least 8 characters long.");
            if (!Regex.IsMatch(value, @"[A-Z]"))
                return ValidationResult.Fail("Password must contain at least one uppercase letter.");
            if (!Regex.IsMatch(value, @"[a-z]"))
                return ValidationResult.Fail("Password must contain at least one lowercase letter.");
            if (!Regex.IsMatch(value, @"\d"))
                return ValidationResult.Fail("Password must contain at least one digit.");
            return ValidationResult.Ok();
        }

        // ─── Composite Validation ─────────────────────────────────────────────────

        /// <summary>Runs all provided validations and aggregates errors.</summary>
        public static ValidationResult Combine(params ValidationResult[] results)
        {
            var errors = new List<string>();
            foreach (var r in results)
                if (!r.IsValid) errors.AddRange(r.Errors);
            return errors.Count == 0 ? ValidationResult.Ok() : new ValidationResult(false, errors);
        }
    }

    // ─── Result Type ──────────────────────────────────────────────────────────────

    /// <summary>Lightweight value object returned by all validation methods.</summary>
    public sealed class ValidationResult
    {
        public bool IsValid { get; }
        public IReadOnlyList<string> Errors { get; }

        public ValidationResult(bool isValid, IReadOnlyList<string>? errors = null)
        {
            IsValid = isValid;
            Errors  = errors ?? Array.Empty<string>();
        }

        public static ValidationResult Ok()   => new(true);
        public static ValidationResult Fail(string message) => new(false, new[] { message });

        /// <summary>Returns all error messages joined by newline.</summary>
        public string ErrorMessage => string.Join(Environment.NewLine, Errors);
    }
}

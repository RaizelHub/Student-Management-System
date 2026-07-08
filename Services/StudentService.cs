using System.Collections.Generic;
using System.Data;
using StudentAttendanceSysttem.Models;
using StudentAttendanceSysttem.Repositories;
using StudentAttendanceSysttem.Utilities;

namespace StudentAttendanceSysttem.Services
{
    /// <summary>
    /// Business logic layer for student management.
    /// Validates input and delegates persistence to <see cref="StudentRepository"/>.
    /// </summary>
    public class StudentService
    {
        private readonly StudentRepository _repo;
        private readonly AuditService      _audit;

        public StudentService(StudentRepository repo, AuditService audit)
        {
            _repo  = repo;
            _audit = audit;
        }

        // ─── Read ─────────────────────────────────────────────────────────────────

        public List<Student> GetAllStudents(bool activeOnly = true) =>
            _repo.GetAll(activeOnly);

        public Student? GetStudentById(int id) => _repo.GetById(id);

        public DataTable GetStudentsAsTable()  => _repo.GetAllAsTable();

        public DataTable SearchStudents(string query) =>
            string.IsNullOrWhiteSpace(query) ? _repo.GetAllAsTable() : _repo.Search(query);

        // ─── Create ───────────────────────────────────────────────────────────────

        public (bool Success, string Message, int NewId) AddStudent(Student student)
        {
            var validation = ValidateStudent(student);
            if (!validation.IsValid) return (false, validation.ErrorMessage, 0);

            if (_repo.StudentNumberExists(student.StudentNumber))
                return (false, $"Student number '{student.StudentNumber}' is already in use.", 0);

            int newId = _repo.Add(student);
            _audit.LogAction("ADD_STUDENT",
                $"Added student: {student.FullName} ({student.StudentNumber})");
            return (true, "Student added successfully.", newId);
        }

        // ─── Update ───────────────────────────────────────────────────────────────

        public (bool Success, string Message) UpdateStudent(Student student)
        {
            var validation = ValidateStudent(student);
            if (!validation.IsValid) return (false, validation.ErrorMessage);

            if (_repo.StudentNumberExists(student.StudentNumber, student.Id))
                return (false, $"Student number '{student.StudentNumber}' is already used by another student.");

            bool ok = _repo.Update(student);
            if (ok) _audit.LogAction("UPDATE_STUDENT",
                $"Updated student: {student.FullName} ({student.StudentNumber})");
            return (ok, ok ? "Student updated successfully." : "Update failed.");
        }

        // ─── Delete ───────────────────────────────────────────────────────────────

        public (bool Success, string Message) DeleteStudent(int id, string name)
        {
            bool ok = _repo.Delete(id);
            if (ok) _audit.LogAction("DELETE_STUDENT", $"Deactivated student: {name} (ID: {id})");
            return (ok, ok ? "Student deactivated successfully." : "Delete failed.");
        }

        // ─── Validation ───────────────────────────────────────────────────────────

        private static ValidationResult ValidateStudent(Student s) =>
            ValidationHelper.Combine(
                ValidationHelper.IsValidStudentNumber(s.StudentNumber),
                ValidationHelper.IsNotEmpty(s.FirstName, "First Name"),
                ValidationHelper.IsNotEmpty(s.LastName,  "Last Name"),
                ValidationHelper.IsNotEmpty(s.Gender,    "Gender"),
                string.IsNullOrWhiteSpace(s.Email) ? ValidationResult.Ok()
                    : ValidationHelper.IsValidEmail(s.Email),
                string.IsNullOrWhiteSpace(s.ContactNumber) ? ValidationResult.Ok()
                    : ValidationHelper.IsValidPhoneNumber(s.ContactNumber)
            );
    }
}

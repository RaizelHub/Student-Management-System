using System.Collections.Generic;
using System.Data;
using StudentAttendanceSysttem.Models;
using StudentAttendanceSysttem.Repositories;
using StudentAttendanceSysttem.Utilities;

namespace StudentAttendanceSysttem.Services
{
    public class CourseService
    {
        private readonly CourseRepository _repo;
        private readonly AuditService     _audit;

        public CourseService(CourseRepository repo, AuditService audit)
        { _repo = repo; _audit = audit; }

        public List<Course> GetAllCourses(bool activeOnly = true) => _repo.GetAll(activeOnly);
        public DataTable    GetCoursesTable()                      => _repo.GetAllAsTable();
        public Course?      GetById(int id)                        => _repo.GetById(id);

        public (bool Success, string Message, int NewId) AddCourse(Course c)
        {
            var v = ValidationHelper.Combine(
                ValidationHelper.IsNotEmpty(c.CourseName, "Course Name"),
                ValidationHelper.IsMaxLength(c.CourseName, 100, "Course Name"));
            if (!v.IsValid) return (false, v.ErrorMessage, 0);
            if (_repo.CourseNameExists(c.CourseName)) return (false, "Course name already exists.", 0);

            int id = _repo.Add(c);
            _audit.LogAction("ADD_COURSE", $"Added course: {c.CourseName}");
            return (true, "Course added.", id);
        }

        public (bool Success, string Message) UpdateCourse(Course c)
        {
            var v = ValidationHelper.IsNotEmpty(c.CourseName, "Course Name");
            if (!v.IsValid) return (false, v.ErrorMessage);
            if (_repo.CourseNameExists(c.CourseName, c.Id)) return (false, "Course name already in use.");

            bool ok = _repo.Update(c);
            if (ok) _audit.LogAction("UPDATE_COURSE", $"Updated course: {c.CourseName}");
            return (ok, ok ? "Course updated." : "Update failed.");
        }

        public (bool Success, string Message) DeleteCourse(int id, string name)
        {
            bool ok = _repo.Delete(id);
            if (ok) _audit.LogAction("DELETE_COURSE", $"Deleted course: {name}");
            return (ok, ok ? "Course deleted." : "Delete failed.");
        }
    }
}

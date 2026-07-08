using System.Collections.Generic;
using System.Data;
using StudentAttendanceSysttem.Models;
using StudentAttendanceSysttem.Repositories;
using StudentAttendanceSysttem.Utilities;

namespace StudentAttendanceSysttem.Services
{
    public class SectionService
    {
        private readonly SectionRepository _repo;
        private readonly AuditService      _audit;

        public SectionService(SectionRepository repo, AuditService audit)
        { _repo = repo; _audit = audit; }

        public List<Section> GetAllSections(bool activeOnly = true) => _repo.GetAll(activeOnly);
        public List<Section> GetByCourse(int courseId)              => _repo.GetByCourse(courseId);
        public DataTable     GetSectionsTable()                     => _repo.GetAllAsTable();

        public (bool Success, string Message, int NewId) AddSection(Section s)
        {
            var v = ValidationHelper.IsNotEmpty(s.SectionName, "Section Name");
            if (!v.IsValid) return (false, v.ErrorMessage, 0);
            int id = _repo.Add(s);
            _audit.LogAction("ADD_SECTION", $"Added section: {s.SectionName}");
            return (true, "Section added.", id);
        }

        public (bool Success, string Message) UpdateSection(Section s)
        {
            var v = ValidationHelper.IsNotEmpty(s.SectionName, "Section Name");
            if (!v.IsValid) return (false, v.ErrorMessage);
            bool ok = _repo.Update(s);
            if (ok) _audit.LogAction("UPDATE_SECTION", $"Updated section: {s.SectionName}");
            return (ok, ok ? "Section updated." : "Update failed.");
        }

        public (bool Success, string Message) DeleteSection(int id, string name)
        {
            bool ok = _repo.Delete(id);
            if (ok) _audit.LogAction("DELETE_SECTION", $"Deleted section: {name}");
            return (ok, ok ? "Section deleted." : "Delete failed.");
        }
    }
}

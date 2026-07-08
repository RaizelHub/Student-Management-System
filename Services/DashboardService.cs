using System.Data;
using StudentAttendanceSysttem.Repositories;

namespace StudentAttendanceSysttem.Services
{
    /// <summary>
    /// Aggregates dashboard statistics for the DashboardForm.
    /// Thin service layer — delegates to DashboardRepository.
    /// </summary>
    public class DashboardService
    {
        private readonly DashboardRepository _repo;
        public DashboardService(DashboardRepository repo) => _repo = repo;

        public DashboardStats GetTodayStats() => new()
        {
            TotalStudents        = _repo.GetTotalStudents(),
            PresentToday         = _repo.GetPresentToday(),
            AbsentToday          = _repo.GetAbsentToday(),
            LateToday            = _repo.GetLateToday(),
            AttendancePercentage = _repo.GetAttendancePercentageToday()
        };

        public DataTable GetRecentAttendance(int limit = 50) =>
            _repo.GetRecentAttendance(limit);

        public DataTable GetWeeklyTrend() =>
            _repo.GetAttendanceTrend(7);
    }

    public class DashboardStats
    {
        public int    TotalStudents        { get; init; }
        public int    PresentToday         { get; init; }
        public int    AbsentToday          { get; init; }
        public int    LateToday            { get; init; }
        public double AttendancePercentage { get; init; }
    }
}

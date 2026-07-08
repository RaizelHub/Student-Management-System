using System;
using System.Data;
using MySqlConnector;
using StudentAttendanceSysttem.Database;

namespace StudentAttendanceSysttem.Repositories
{
    /// <summary>
    /// Provides aggregate statistics for the dashboard screen.
    /// All queries are scoped to the current calendar date.
    /// </summary>
    public class DashboardRepository
    {
        private readonly DatabaseConnection _db;
        private readonly string _dateColumn;
        public DashboardRepository(DatabaseConnection db)
        {
            _db = db;
            // detect date column name
            var candidates = new[] { "date", "attendance_date", "attendance_on", "attendance_datetime", "created_at" };
            foreach (var c in candidates)
            {
                if (db.ColumnExists("attendance", c)) { _dateColumn = c; break; }
            }
            _dateColumn ??= "date";
        }

        public DashboardRepository(DatabaseConnection db, string dateColumn) { _db = db; _dateColumn = dateColumn; }

        private string UseDate(string? alias = null)
        {
            var col = string.IsNullOrWhiteSpace(_dateColumn) ? "date" : _dateColumn;
            return string.IsNullOrWhiteSpace(alias) ? col : $"{alias}.{col}";
        }

        public int GetTotalStudents()
        {
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = new MySqlCommand(
                "SELECT COUNT(*) FROM students", conn);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public int GetPresentToday()
        {
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = new MySqlCommand(
                $"SELECT COUNT(DISTINCT student_id) FROM attendance WHERE DATE({UseDate()}) = CURDATE() AND status = 'Present'", conn);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public int GetAbsentToday()
        {
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = new MySqlCommand(
                $"SELECT COUNT(DISTINCT student_id) FROM attendance WHERE DATE({UseDate()}) = CURDATE() AND status = 'Absent'", conn);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public int GetLateToday()
        {
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = new MySqlCommand(
                $"SELECT COUNT(DISTINCT student_id) FROM attendance WHERE DATE({UseDate()}) = CURDATE() AND status = 'Late'", conn);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public double GetAttendancePercentageToday()
        {
            int total   = GetTotalStudents();
            int present = GetPresentToday() + GetLateToday();
            if (total == 0) return 0;
            return Math.Round((double)present / total * 100, 1);
        }

        public DataTable GetRecentAttendance(int limit = 50)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            var dateCol = UseDate("a");
            string sql = $@"
                SELECT
                    a.id,
                    s.student_number,
                    CONCAT(s.first_name, ' ', IFNULL(s.last_name, '')) AS student_name,
                    {dateCol} AS date,
                    a.time_in,
                    a.time_out,
                    a.status
                FROM attendance a
                INNER JOIN students s ON a.student_id = s.id
                ORDER BY {dateCol} DESC, a.time_in DESC
                LIMIT @limit";

            using var adapter = new MySqlDataAdapter(sql, conn);
            adapter.SelectCommand!.Parameters.AddWithValue("@limit", limit);
            var dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        public DataTable GetAttendanceTrend(int days = 7)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            var dateCol = UseDate("");
            var sql = $@"
                SELECT
                    DATE({dateCol})                                          AS attendance_date,
                    SUM(CASE WHEN status='Present' THEN 1 ELSE 0 END)  AS present_count,
                    SUM(CASE WHEN status='Absent'  THEN 1 ELSE 0 END)  AS absent_count,
                    SUM(CASE WHEN status='Late'    THEN 1 ELSE 0 END)  AS late_count
                FROM attendance
                WHERE {dateCol} >= DATE_SUB(CURDATE(), INTERVAL @days DAY)
                GROUP BY DATE({dateCol})
                ORDER BY DATE({dateCol})";

            using var adapter = new MySqlDataAdapter(sql, conn);
            adapter.SelectCommand!.Parameters.AddWithValue("@days", days);
            var dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }
    }
}

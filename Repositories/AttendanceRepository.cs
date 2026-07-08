using System;
using System.Collections.Generic;
using System.Data;
using MySqlConnector;
using StudentAttendanceSysttem.Database;
using StudentAttendanceSysttem.Models;

namespace StudentAttendanceSysttem.Repositories
{
    /// <summary>
    /// Data access for the <c>attendance</c> table.
    /// ASSUMPTION: id, student_id, date, time_in, time_out, status, remarks, created_at
    /// </summary>
    public class AttendanceRepository
    {
        private readonly DatabaseConnection _db;
        private readonly string _dateColumn;
        public AttendanceRepository(DatabaseConnection db)
        {
            _db = db;
            // Detect date column name at runtime and fall back to 'date'
            var candidates = new[] { "date", "attendance_date", "attendance_on", "attendance_datetime", "created_at" };
            foreach (var c in candidates)
            {
                if (db.ColumnExists("attendance", c)) { _dateColumn = c; break; }
            }
            _dateColumn ??= "date"; // fallback
        }

        public bool HasAttendanceToday(int studentId)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            var sql = $"SELECT COUNT(*) FROM attendance WHERE student_id=@sid AND DATE({_dateColumn})=CURDATE()";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@sid", studentId);
            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }

        public Attendance? GetTodayRecord(int studentId)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            const string sql = @"
                SELECT a.*, CONCAT(s.first_name,' ',s.last_name) AS student_name,
                       s.student_number, c.course_name, sec.section_name
                FROM   attendance a
                INNER JOIN students  s   ON a.student_id=s.id
                LEFT  JOIN courses   c   ON s.course_id=c.id
                LEFT  JOIN sections  sec ON s.section_id=sec.id
                WHERE  a.student_id=@sid AND DATE(a.{_dateColumn})=CURDATE()
                LIMIT 1";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@sid", studentId);
            using var r = cmd.ExecuteReader();
            return r.Read() ? MapRecord(r) : null;
        }

        public int TimeIn(int studentId, string status, string remarks = "")
        {
            using var conn = _db.GetConnection();
            conn.Open();
            var sql = $@"INSERT INTO attendance (student_id, {_dateColumn}, time_in, status, remarks, created_at)
                                 VALUES (@sid, CURDATE(), CURTIME(), @status, @remarks, NOW());
                                 SELECT LAST_INSERT_ID();";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@sid",     studentId);
            cmd.Parameters.AddWithValue("@status",  status);
            cmd.Parameters.AddWithValue("@remarks", remarks);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public bool TimeOut(int attendanceId)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            const string sql = "UPDATE attendance SET time_out=CURTIME() WHERE id=@id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", attendanceId);
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool MarkAbsent(int studentId, string remarks = "")
        {
            if (HasAttendanceToday(studentId)) return false;
            using var conn = _db.GetConnection();
            conn.Open();
            var sql = $@"INSERT INTO attendance (student_id, {_dateColumn}, status, remarks, created_at)
                                 VALUES (@sid, CURDATE(), 'Absent', @remarks, NOW())";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@sid",     studentId);
            cmd.Parameters.AddWithValue("@remarks", remarks);
            return cmd.ExecuteNonQuery() > 0;
        }

        public DataTable GetByDate(DateTime date)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            const string sql = @"
                SELECT a.id, s.student_number,
                       CONCAT(s.first_name,' ',s.last_name) AS student_name,
                       c.course_name, sec.section_name,
                       a.date, a.time_in, a.time_out, a.status, a.remarks
                FROM   attendance a
                INNER JOIN students  s   ON a.student_id=s.id
                LEFT  JOIN courses   c   ON s.course_id=c.id
                LEFT  JOIN sections  sec ON s.section_id=sec.id
                WHERE  DATE(a.{_dateColumn})=@date
                ORDER BY a.time_in DESC";
            using var adapter = new MySqlDataAdapter(sql, conn);
            adapter.SelectCommand!.Parameters.AddWithValue("@date", date.Date);
            var dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        public DataTable GetByDateRange(DateTime from, DateTime to, int? courseId = null, int? sectionId = null)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            string sql = @"
                SELECT a.id, s.student_number,
                       CONCAT(s.first_name,' ',s.last_name) AS student_name,
                       c.course_name, sec.section_name,
                       a.date, a.time_in, a.time_out, a.status, a.remarks
                FROM   attendance a
                INNER JOIN students  s   ON a.student_id=s.id
                LEFT  JOIN courses   c   ON s.course_id=c.id
                LEFT  JOIN sections  sec ON s.section_id=sec.id
                WHERE  a.{_dateColumn} BETWEEN @from AND @to";

            if (courseId.HasValue)   sql += " AND s.course_id=@cid";
            if (sectionId.HasValue)  sql += " AND s.section_id=@sid";
            sql += " ORDER BY a.date DESC, s.last_name";

            using var adapter = new MySqlDataAdapter(sql, conn);
            adapter.SelectCommand!.Parameters.AddWithValue("@from", from.Date);
            adapter.SelectCommand.Parameters.AddWithValue("@to",   to.Date);
            if (courseId.HasValue)  adapter.SelectCommand.Parameters.AddWithValue("@cid", courseId.Value);
            if (sectionId.HasValue) adapter.SelectCommand.Parameters.AddWithValue("@sid", sectionId.Value);

            var dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        public DataTable GetStudentHistory(int studentId)
        {
            using var conn = _db.GetConnection();
            conn.Open();
                var sql = $@"
                SELECT {_dateColumn} AS date, time_in, time_out, status, remarks
                FROM   attendance WHERE student_id=@sid ORDER BY {_dateColumn} DESC";
            using var adapter = new MySqlDataAdapter(sql, conn);
            adapter.SelectCommand!.Parameters.AddWithValue("@sid", studentId);
            var dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        private static Attendance MapRecord(MySqlDataReader r) => new()
        {
            Id          = r.GetInt32("id"),
            StudentId   = r.GetInt32("student_id"),
            StudentName = r.IsDBNull(r.GetOrdinal("student_name")) ? "" : r.GetString("student_name"),
            StudentNumber = r.IsDBNull(r.GetOrdinal("student_number")) ? "" : r.GetString("student_number"),
            CourseName  = r.IsDBNull(r.GetOrdinal("course_name")) ? "" : r.GetString("course_name"),
            SectionName = r.IsDBNull(r.GetOrdinal("section_name")) ? "" : r.GetString("section_name"),
            Date        = r.GetDateTime("date"),
            TimeIn      = r.IsDBNull(r.GetOrdinal("time_in"))  ? null : r.GetTimeSpan("time_in"),
            TimeOut     = r.IsDBNull(r.GetOrdinal("time_out")) ? null : r.GetTimeSpan("time_out"),
            Status      = r.GetString("status"),
            Remarks     = r.IsDBNull(r.GetOrdinal("remarks")) ? "" : r.GetString("remarks"),
            CreatedAt   = r.GetDateTime("created_at"),
        };
    }
}

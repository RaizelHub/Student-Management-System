using System;
using System.Collections.Generic;
using System.Data;
using MySqlConnector;
using StudentAttendanceSysttem.Database;
using StudentAttendanceSysttem.Models;

namespace StudentAttendanceSysttem.Repositories
{
    /// <summary>
    /// Data access for the <c>sections</c> table.
    /// ASSUMPTION: id, section_name, course_id, year_level, school_year, is_active, created_at, updated_at
    /// </summary>
    public class SectionRepository
    {
        private readonly DatabaseConnection _db;
        public SectionRepository(DatabaseConnection db) => _db = db;

        public List<Section> GetAll(bool activeOnly = true)
        {
            var list = new List<Section>();
            using var conn = _db.GetConnection();
            conn.Open();
            string sql = @"
                SELECT s.*, c.course_name
                FROM   sections s
                LEFT JOIN courses c ON s.course_id = c.id"
                + (activeOnly ? " WHERE s.is_active = 1" : "")
                + " ORDER BY s.section_name";
            using var cmd    = new MySqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read()) list.Add(Map(reader));
            return list;
        }

        public List<Section> GetByCourse(int courseId)
        {
            var list = new List<Section>();
            using var conn = _db.GetConnection();
            conn.Open();
            const string sql = "SELECT s.*, c.course_name FROM sections s LEFT JOIN courses c ON s.course_id=c.id WHERE s.course_id=@cid AND s.is_active=1 ORDER BY s.section_name";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@cid", courseId);
            using var reader = cmd.ExecuteReader();
            while (reader.Read()) list.Add(Map(reader));
            return list;
        }

        public DataTable GetAllAsTable()
        {
            using var conn = _db.GetConnection();
            conn.Open();
            const string sql = @"SELECT s.id, s.section_name, c.course_name, s.year_level, s.school_year, s.is_active
                                 FROM sections s LEFT JOIN courses c ON s.course_id=c.id WHERE s.is_active=1 ORDER BY s.section_name";
            using var adapter = new MySqlDataAdapter(sql, conn);
            var dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        public int Add(Section s)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            const string sql = @"INSERT INTO sections (section_name, course_id, year_level, school_year, is_active, created_at, updated_at)
                                 VALUES (@name, @cid, @yl, @sy, 1, NOW(), NOW()); SELECT LAST_INSERT_ID();";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@name", s.SectionName);
            cmd.Parameters.AddWithValue("@cid",  s.CourseId > 0 ? s.CourseId : DBNull.Value);
            cmd.Parameters.AddWithValue("@yl",   s.YearLevel);
            cmd.Parameters.AddWithValue("@sy",   s.SchoolYear);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public bool Update(Section s)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            const string sql = "UPDATE sections SET section_name=@name, course_id=@cid, year_level=@yl, school_year=@sy, updated_at=NOW() WHERE id=@id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@name", s.SectionName);
            cmd.Parameters.AddWithValue("@cid",  s.CourseId > 0 ? s.CourseId : DBNull.Value);
            cmd.Parameters.AddWithValue("@yl",   s.YearLevel);
            cmd.Parameters.AddWithValue("@sy",   s.SchoolYear);
            cmd.Parameters.AddWithValue("@id",   s.Id);
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Delete(int id)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            const string sql = "UPDATE sections SET is_active=0, updated_at=NOW() WHERE id=@id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            return cmd.ExecuteNonQuery() > 0;
        }

        private static Section Map(MySqlDataReader r) => new()
        {
            Id          = r.GetInt32("id"),
            SectionName = r.GetString("section_name"),
            CourseId    = r.IsDBNull(r.GetOrdinal("course_id")) ? 0 : r.GetInt32("course_id"),
            CourseName  = r.IsDBNull(r.GetOrdinal("course_name")) ? "" : r.GetString("course_name"),
            YearLevel   = r.IsDBNull(r.GetOrdinal("year_level")) ? "" : r.GetString("year_level"),
            SchoolYear  = r.IsDBNull(r.GetOrdinal("school_year")) ? "" : r.GetString("school_year"),
            IsActive    = r.GetBoolean("is_active"),
            CreatedAt   = r.GetDateTime("created_at"),
            UpdatedAt   = r.GetDateTime("updated_at"),
        };
    }
}

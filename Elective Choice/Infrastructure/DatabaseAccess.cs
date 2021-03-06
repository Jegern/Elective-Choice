using System;
using System.Collections.Generic;
using System.Globalization;
using Elective_Choice.Models;
using Npgsql;

namespace Elective_Choice.Infrastructure;

public static class DatabaseAccess
{
    private static NpgsqlConnection SqlConnection { get; } =
        new(@"Server=surus.db.elephantsql.com;
              User Id=zmfwlqkl;
              Password=mTKr9LzJYGiSitrt5zvTSN8yq9sbfXcj;
              Database=zmfwlqkl;");

    public static bool PersonIsStudent(string id)
    {
        SqlConnection.Open();

        var student = new NpgsqlCommand(
            $@"SELECT id
                      FROM students
                      WHERE id = '{id}'",
            SqlConnection).ExecuteScalar();

        SqlConnection.Close();

        return student is not null;
    }

    public static bool PersonIsAdmin(string id)
    {
        SqlConnection.Open();

        var admin = new NpgsqlCommand(
            $@"SELECT id
                      FROM admins
                      WHERE id = '{id}'",
            SqlConnection).ExecuteScalar();

        SqlConnection.Close();

        return admin is not null;
    }

    public static string GetPersonNameBy(string id)
    {
        SqlConnection.Open();

        var name = new NpgsqlCommand(
            $@"SELECT name
                      FROM (
                          SELECT id, name
                          FROM admins
                          UNION 
                          SELECT id, name
                          FROM students) AS names
                      WHERE id = '{id}'",
            SqlConnection).ExecuteScalar()?.ToString();

        SqlConnection.Close();

        return name!;
    }

    public static List<Elective> GetSemesterElectives(int year, bool spring)
    {
        SqlConnection.Open();

        var electives = new List<Elective>();
        var reader = new NpgsqlCommand(
            $@"SELECT name, capacity
                      FROM electives JOIN (
                          SELECT elective_id
                          FROM past_semesters
                          WHERE year = {year} AND spring = {spring}
                          GROUP BY elective_id) AS semester_electives ON electives.id = elective_id",
            SqlConnection).ExecuteReader();
        while (reader.Read())
            electives.Add(new Elective(reader.GetString(0), (uint) reader.GetInt32(1)));

        SqlConnection.Close();

        return electives;
    }

    public static List<Semester> GetSemesters()
    {
        SqlConnection.Open();

        var semesters = new List<Semester>();
        var reader = new NpgsqlCommand(
            @"SELECT year, spring, COUNT(elective_id)
                     FROM (
                         SELECT year, spring, elective_id
                         FROM past_semesters
                         GROUP BY year, spring, elective_id
                         ORDER BY year DESC) AS past_semesters
                     GROUP BY year, spring",
            SqlConnection).ExecuteReader();
        while (reader.Read())
            semesters.Add(new Semester(
                reader.GetInt32(0),
                reader.GetBoolean(1),
                reader.GetInt32(2)));

        SqlConnection.Close();

        return semesters;
    }

    public static int[][] GetElectiveStatistics(string name)
    {
        SqlConnection.Open();

        var values = new[] {new int[5], new int[5], new int[5], new int[5], new int[5]};
        var performances = new[] {3.0, 4.0, 4.75, 5.0};
        for (var i = 0; i < performances.Length - 1; i++)
        {
            var reader = new NpgsqlCommand(
                $@"SELECT priority, Count(*) AS number_of_elections
                          FROM selected_electives
                              JOIN electives ON selected_electives.elective_id = electives.id
                              JOIN students ON selected_electives.student_id = students.id
                          WHERE electives.name = '{name}' AND
                                performance >= {performances[i].ToString(CultureInfo.InvariantCulture)} AND
                                performance < {performances[i + 1].ToString(CultureInfo.InvariantCulture)}
                          GROUP BY priority",
                SqlConnection).ExecuteReader();
            while (reader.Read())
                values[i][reader.GetInt32(0) - 1] = reader.GetInt32(1);

            reader.Close();
        }

        SqlConnection.Close();

        return values;
    }

    public static int[][] GetElectiveStatistics(string name, int year, bool spring)
    {
        SqlConnection.Open();

        var values = new[] {new int[5], new int[5], new int[5], new int[5], new int[5]};
        var performances = new[] {3.0, 4.0, 4.75, 5.0};
        for (var i = 0; i < performances.Length - 1; i++)
        {
            var reader = new NpgsqlCommand(
                $@"SELECT priority, Count(*) AS number_of_elections
                          FROM past_semesters
                              JOIN electives ON past_semesters.elective_id = electives.id
                              JOIN students ON past_semesters.student_id = students.id
                          WHERE electives.name = '{name}' AND year = {year} AND spring = {spring}
                              AND performance >= {performances[i]} AND performance < {performances[i + 1]}
                          GROUP BY priority",
                SqlConnection).ExecuteReader();
            while (reader.Read())
                values[i][reader.GetInt32(0) - 1] = reader.GetInt32(1);

            reader.Close();
        }

        SqlConnection.Close();

        return values;
    }

    public static int GetRecommendCapacity(string name)
    {
        SqlConnection.Open();

        var reader = new NpgsqlCommand(
            $@"SELECT count(id) AS counts, capacity
               FROM selected_electives 
                   JOIN electives ON selected_electives.elective_id = electives.id
               WHERE exist AND name = '{name}'
               GROUP BY capacity",
            SqlConnection).ExecuteReader();

        reader.Read();
        var counts = reader.GetInt32(0);
        var capacity = reader.GetInt32(1);
        
        SqlConnection.Close();

        return counts < capacity ? capacity / 2 < 15 ? capacity : capacity / 2 : Math.Min(counts / 3, 2 * capacity);
    }

    public static List<Elective> GetIncompleteElectives()
    {
        SqlConnection.Open();

        var electives = new List<Elective>();
        var reader = new NpgsqlCommand(
            @"SELECT name, capacity, counts
                         FROM (
                             SELECT count(id) AS counts, name, capacity
                             FROM selected_electives JOIN (
                                 SELECT id, name, capacity
                                 FROM electives
                                 WHERE exist) AS current_electives ON elective_id = id
                             GROUP BY id, name, capacity) AS elective_counts
                     WHERE counts < 0.5 * capacity OR counts < 15",
            SqlConnection).ExecuteReader();
        while (reader.Read())
            electives.Add(new Elective(
                reader.GetString(0),
                (uint) reader.GetInt32(1),
                problem: "Incomplete"));

        SqlConnection.Close();

        return electives;
    }

    public static List<Elective> GetOverflowedElectives()
    {
        SqlConnection.Open();

        var electives = new List<Elective>();
        var reader = new NpgsqlCommand(
            @"SELECT name, capacity, counts
                         FROM (
                             SELECT count(id) AS counts, name, capacity
                             FROM selected_electives JOIN (
                                 SELECT id, name, capacity
                                 FROM electives
                                 WHERE exist) AS current_electives ON elective_id = id
                             GROUP BY id, name, capacity) AS elective_counts
                     WHERE counts > 3 * capacity",
            SqlConnection).ExecuteReader();
        while (reader.Read())
            electives.Add(new Elective(
                reader.GetString(0),
                (uint) reader.GetInt32(1),
                problem: "Overflowed"));

        SqlConnection.Close();

        return electives;
    }

    public static void UpdateElectiveCapacity(Elective elective)
    {
        SqlConnection.Open();

        new NpgsqlCommand(
            $@"UPDATE electives
                      SET capacity = {elective.Capacity}
                      WHERE name = '{elective.Name}'",
            SqlConnection).ExecuteNonQuery();

        SqlConnection.Close();
    }

    public static List<Elective> GetElectivesForDay(int day)
    {
        SqlConnection.Open();

        var electives = new List<Elective>();
        var reader = new NpgsqlCommand(
            $@"SELECT name, capacity, annotation
                      FROM electives
                          JOIN elective_days ON electives.id = elective_days.elective_id
                      WHERE exist AND day_of_week = {day}",
            SqlConnection).ExecuteReader();
        while (reader.Read())
            electives.Add(new Elective(
                reader.GetString(0),
                (uint) reader.GetInt32(1),
                reader.GetString(2)));

        SqlConnection.Close();

        return electives;
    }

    public static void AddStudentElective(string studentId, string electiveName, int priority)
    {
        SqlConnection.Open();

        var electiveId = new NpgsqlCommand(
            $@"SELECT id
                      FROM electives
                      WHERE name = '{electiveName}'",
            SqlConnection).ExecuteScalar();
        new NpgsqlCommand(
            $@"INSERT INTO selected_electives
                      VALUES ('{studentId}', {electiveId}, {priority})",
            SqlConnection).ExecuteNonQuery();

        SqlConnection.Close();
    }

    public static void RemoveStudentElective(string studentId, string electiveName)
    {
        SqlConnection.Open();

        var electiveId = new NpgsqlCommand(
            $@"SELECT id
                      FROM electives
                      WHERE name = '{electiveName}'",
            SqlConnection).ExecuteScalar();
        new NpgsqlCommand(
            $@"DELETE FROM selected_electives
                      WHERE student_id = '{studentId}' AND elective_id = {electiveId}",
            SqlConnection).ExecuteNonQuery();

        SqlConnection.Close();
    }

    public static List<Elective> GetStudentElectives(string studentId)
    {
        SqlConnection.Open();

        var electives = new List<Elective>();
        var reader = new NpgsqlCommand(
            $@"SELECT name, capacity, annotation, day_of_week, priority
                      FROM selected_electives
                          JOIN electives ON selected_electives.elective_id = electives.id
                          JOIN elective_days ON selected_electives.elective_id = elective_days.elective_id
                      WHERE student_id = '{studentId}'
                      ORDER BY priority",
            SqlConnection).ExecuteReader();
        while (reader.Read())
            electives.Add(new Elective(
                reader.GetString(0),
                (uint) reader.GetInt32(1),
                reader.GetString(2),
                day: reader.GetInt32(3),
                priority: reader.GetInt32(4)));

        SqlConnection.Close();

        return electives;
    }

    public static void UpdateStudentElectivePriority(string studentId, List<Elective> electives)
    {
        SqlConnection.Open();

        foreach (var elective in electives)
        {
            new NpgsqlCommand(
                $@"UPDATE selected_electives
                          SET priority = {elective.Priority}
                          FROM electives
                          WHERE selected_electives.elective_id = electives.id AND
                              selected_electives.student_id = '{studentId}' AND electives.name = '{elective.Name}'",
                SqlConnection).ExecuteNonQuery();
        }

        SqlConnection.Close();
    }

    public static List<DateTime> GetAlgorithmSettings()
    {
        SqlConnection.Open();

        var settings = new List<DateTime>();
        var reader = new NpgsqlCommand(
            @"SELECT *
                     FROM settings",
            SqlConnection).ExecuteReader();
        reader.Read();
        settings.Add(reader.GetDateTime(0));
        settings.Add(reader.GetDateTime(1));
        settings.Add(reader.GetDateTime(2));

        SqlConnection.Close();

        return settings;
    }

    public static void UpdateAlgorithmSettings(DateTime? startChoices, DateTime? endChoices, DateTime? startAlgorithm)
    {
        SqlConnection.Open();

        new NpgsqlCommand(
            $@"UPDATE settings
                      SET start_choices = '{startChoices!.Value.ToString("u")}',
                          end_choices = '{endChoices!.Value.ToString("u")}',
                          start_algorithm = '{startAlgorithm!.Value.ToString("u")}'",
            SqlConnection).ExecuteNonQuery();

        SqlConnection.Close();
    }

    public static List<List<Elective>> GetStudentResultElectives(string studentId)
    {
        SqlConnection.Open();
        var electives = new List<List<Elective>>();

        var reader = new NpgsqlCommand(
            $@"SELECT name, priority, year, spring
                      FROM past_semesters
                          JOIN electives ON past_semesters.elective_id = electives.id
                      WHERE student_id = '{studentId}' AND assigned
                      ORDER BY year",
            SqlConnection).ExecuteReader();

        var year = 0;
        var spring = false;
        var semesterElectives = new List<Elective>();
        while (reader.Read())
        {
            if (reader.GetInt32(2) != year || reader.GetBoolean(3) != spring)
            {
                if (semesterElectives.Count > 0)
                    electives.Add(semesterElectives);
                year = reader.GetInt32(2);
                spring = reader.GetBoolean(3);
                semesterElectives = new List<Elective>
                {
                    new(reader.GetString(0),
                        priority: reader.GetInt32(1),
                        year: year,
                        spring: spring)
                };
            }
            else
            {
                year = reader.GetInt32(2);
                spring = reader.GetBoolean(3);
                semesterElectives.Add(new Elective(reader.GetString(0),
                    priority: reader.GetInt32(1),
                    year: year,
                    spring: spring));
            }
        }
        electives.Add(semesterElectives);
        SqlConnection.Close();

        return electives;
    }
}
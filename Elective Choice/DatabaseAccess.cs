using System.Collections.Generic;
using Elective_Choice.Models;
using Npgsql;

namespace Elective_Choice;

public static class DatabaseAccess
{
    private static NpgsqlConnection SqlConnection { get; } =
        new("Server=localhost;Port=5432;User Id=postgres;Password=12345;Database=electives;");

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
            electives.Add(new Elective(reader.GetString(0), reader.GetInt32(1)));

        SqlConnection.Close();

        return electives;
    }

    public static List<Semester> GetSemesters()
    {
        SqlConnection.Open();

        var semesters = new List<Semester>();
        // TODO: Исправить подсчет количества элективов 
        var reader = new NpgsqlCommand(
            @"SELECT year, spring, COUNT(elective_id)
                     FROM past_semesters
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
        var performances = new[] { 3.0, 4.0, 4.75, 5.0 };
        for (var i = 0; i < performances.Length - 1; i++)
        {
            var reader = new NpgsqlCommand(
                $@"SELECT priority, Count(*) AS number_of_elections
                          FROM selected_electives
                              JOIN electives ON selected_electives.elective_id = electives.id
                              JOIN students ON selected_electives.student_id = students.id
                          WHERE electives.name = '{name}'
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

    public static int[][] GetElectiveStatistics(string name, int year, bool spring)
    {
        SqlConnection.Open();

        var values = new[] {new int[5], new int[5], new int[5], new int[5], new int[5]};
        var performances = new[] { 3.0, 4.0, 4.75, 5.0 };
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

    public static List<Elective> GetIncompleteElectives()
    {
        SqlConnection.Open();

        var electives = new List<Elective>();
        var reader = new NpgsqlCommand(
            @"SELECT name, capacity
                         FROM (
                             SELECT count(id) AS counts, name, capacity
                             FROM selected_electives JOIN (
                                 SELECT id, name, capacity
                                 FROM electives
                                 WHERE exist) AS current_electives ON elective_id = id
                             WHERE assigned = false
                             GROUP BY id, name, capacity) AS elective_counts
                     WHERE counts < 0.8 * capacity OR counts < 15",
            SqlConnection).ExecuteReader();
        while (reader.Read())
            electives.Add(new Elective(reader.GetString(0), reader.GetInt32(1)));

        SqlConnection.Close();

        return electives;
    }

    public static List<Elective> GetOverflowedElectives()
    {
        SqlConnection.Open();

        var electives = new List<Elective>();
        var reader = new NpgsqlCommand(
            @"SELECT name, capacity
                         FROM (
                             SELECT count(id) AS counts, name, capacity
                             FROM selected_electives JOIN (
                                 SELECT id, name, capacity
                                 FROM electives
                                 WHERE exist) AS current_electives ON elective_id = id
                             WHERE assigned = false
                             GROUP BY id, name, capacity) AS elective_counts
                     WHERE counts > 3 * capacity",
            SqlConnection).ExecuteReader();
        while (reader.Read())
            electives.Add(new Elective(reader.GetString(0), reader.GetInt32(1)));

        SqlConnection.Close();

        return electives;
    }
}
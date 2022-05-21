﻿using System.Collections.Generic;
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

    public static List<Elective> GetCurrentElectives()
    {
        SqlConnection.Open();

        var electives = new List<Elective>();
        var reader = new NpgsqlCommand(
            @"SELECT name, capacity 
                     FROM electives
                     WHERE exist
                     ORDER BY name",
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
        var reader = new NpgsqlCommand(
            @"SELECT year, spring
                     FROM past_semesters
                     GROUP BY year, spring
                     ORDER BY year DESC", 
            SqlConnection).ExecuteReader();
        while (reader.Read())
            semesters.Add(new Semester(reader.GetInt32(0), reader.GetBoolean(1)));

        SqlConnection.Close();

        return semesters;
    }

    public static List<int>[] GetElectiveStatistics(string name)
    {
        SqlConnection.Open();

        var values = new List<int>[] {new(), new(), new(), new(), new()};
        for (double i = 0d, performance = 3d; performance <= 5d; i++, performance += 0.5)
        {
            var reader = new NpgsqlCommand(
                $@"SELECT priority, Count(*) AS number_of_elections
                          FROM selected_electives
                              JOIN electives ON selected_electives.elective_id = electives.id
                              JOIN students ON selected_electives.student_id = students.id
                          WHERE electives.name = '{name}'
                              AND performance >= {performance} AND performance < {performance + 0.5}
                          GROUP BY priority",
                SqlConnection).ExecuteReader();
            for (var priority = 1; priority <= 5; priority++)
            {
                if (reader.Read() && reader.GetInt32(0) == priority)
                    values[(int) i].Add(reader.GetInt32(1));
                else
                    values[(int) i].Add(0);
            }

            reader.Close();
        }

        SqlConnection.Close();

        return values;
    }

    public static List<int>[] GetElectiveStatistics(string name, int year, bool spring)
    {
        SqlConnection.Open();

        var values = new List<int>[5];
        for (double i = 0d, performance = 3d; performance <= 5d; i++, performance += 0.5)
        {
            var reader = new NpgsqlCommand(
                $@"SELECT priority, Count(*) AS number_of_elections
                          FROM past_semesters
                              JOIN electives ON past_semesters.elective_id = electives.id
                              JOIN students ON past_semesters.student_id = students.id
                          WHERE electives.name = '{name}' AND year = {year} AND spring = {spring}
                              AND performance >= {performance} AND performance < {performance + 0.5}
                          GROUP BY priority",
                SqlConnection).ExecuteReader();
            for (var priority = 1; priority <= 5; priority++)
            {
                if (reader.Read() && reader.GetInt32(0) == priority)
                    values[(int) i].Add(reader.GetInt32(1));
                else
                    values[(int) i].Add(0);
            }

            reader.Close();
        }

        SqlConnection.Close();

        return values;
    }
}
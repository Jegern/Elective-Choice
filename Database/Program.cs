using System.Reflection;
using Npgsql;

namespace Database
{
    public static class InterfaceData
    {
        private static NpgsqlConnection SqlConnection { get; } =
            new("Server=localhost;Port=5432;User Id=postgres;Password=12345;Database=electives;");

        private static void GenerateStudents(int numberOfStudents = 5084)
        {
            var studentExams = ReadStudentExams();
            var marks = ReadMarks();

            var studentMarks = new Dictionary<string, List<int>>();
            for (var i = 1; i < studentExams.Count; i++)
                if (studentMarks.ContainsKey(studentExams[i]))
                    studentMarks[studentExams[i]].Add(marks[i]);
                else
                    studentMarks.Add(studentExams[i], new List<int> {marks[i]});

            WriteStudents(studentMarks, numberOfStudents);
        }

        private static List<string> ReadStudentExams()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var studentExams = new List<string>();
            using var stream = assembly.GetManifestResourceStream("Database.Students.X_train.csv");
            using var reader = new StreamReader(stream!);
            while (!reader.EndOfStream)
                studentExams.Add(reader.ReadLine()!.Split(',')[1]);
            return studentExams;
        }

        private static List<int> ReadMarks()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var marks = new List<int>();
            using var stream = assembly.GetManifestResourceStream("Database.Students.y_train.csv");
            using var reader = new StreamReader(stream!);
            while (!reader.EndOfStream)
                marks.Add(Convert.ToInt32(reader.ReadLine()!.Split(',')[1]));
            return marks;
        }

        private static void WriteStudents(Dictionary<string, List<int>> studentMarks, int numberOfStudents)
        {
            SqlConnection.Open();

            for (var i = 0; i < studentMarks.Count; i++)
            {
                var (key, value) = studentMarks.ElementAt(i);
                new NpgsqlCommand(
                    $@"INSERT INTO students 
                              VALUES ('{key.PadLeft(10, '0')}',
                                      {value.Average()},
                                      'Студент {i + 1}',
                                      {i < numberOfStudents})",
                    SqlConnection).ExecuteNonQuery();
            }

            SqlConnection.Close();
        }

        private static void DistributeElectivesByDay(int firstDay = 2, int lastDay = 5)
        {
            var electives = ReadElectivesId();
            var electiveDays = electives.ToDictionary(
                x => x,
                _ => new Random().Next(firstDay, lastDay + 1));
            WriteElectiveDays(electiveDays);
        }

        private static List<int> ReadElectivesId()
        {
            SqlConnection.Open();

            var electives = new List<int>();
            var reader = new NpgsqlCommand(@"SELECT id 
                                                    FROM electives 
                                                    ORDER BY id",
                SqlConnection).ExecuteReader();
            while (reader.Read())
                electives.Add(reader.GetInt32(0));
            reader.Close();

            SqlConnection.Close();

            return electives;
        }

        private static void WriteElectiveDays(Dictionary<int, int> electiveDays)
        {
            SqlConnection.Open();

            foreach (var (key, value) in electiveDays)
                new NpgsqlCommand(
                    $@"INSERT INTO elective_days 
                              VALUES ({key}, {value})",
                    SqlConnection).ExecuteNonQuery();

            SqlConnection.Close();
        }

        private static void GenerateChoices()
        {
            var students = ReadStudentId();
            var electives = ReadElectivesId();

            var studentChoices = new Dictionary<string, int[]>();
            foreach (var student in students)
            {
                var choices = Enumerable.Range(1, electives.Count)
                    .OrderBy(_ => new Random().Next())
                    .Take(5)
                    .ToArray();
                studentChoices.Add(student, choices);
            }

            WriteSelectedElectives(studentChoices);
        }

        private static List<string> ReadStudentId()
        {
            SqlConnection.Open();

            var students = new List<string>();
            var reader = new NpgsqlCommand(@"SELECT id 
                                                    FROM students",
                SqlConnection).ExecuteReader();
            while (reader.Read())
                students.Add(reader.GetString(0));
            reader.Close();

            SqlConnection.Close();

            return students;
        }

        private static void WriteSelectedElectives(Dictionary<string, int[]> studentChoices)
        {
            SqlConnection.Open();

            foreach (var (student, choices) in studentChoices)
                for (var i = 0; i < choices.Length; i++)
                    new NpgsqlCommand(
                        $@"INSERT INTO selected_electives 
                                  VALUES ('{student}', {choices[i]}, {i + 1}, {false})",
                        SqlConnection).ExecuteNonQuery();

            SqlConnection.Close();
        }

        public static void Main(string[] args)
        {
        }
    }
}
using System.Reflection;
using Npgsql;
using Numpy;

namespace Database
{
    public static class InterfaceData
    {
        // private static NpgsqlConnection SqlConnection { get; } =
        //     new(@"Server=surus.db.elephantsql.com;
        //           User Id=zmfwlqkl;
        //           Password=mTKr9LzJYGiSitrt5zvTSN8yq9sbfXcj;
        //           Database=zmfwlqkl;");

        private static NpgsqlConnection SqlConnection { get; } =
            new(@"Server=localhost;
                  User Id=postgres;
                  Password=12345;
                  Database=electives;");

        private struct Elective
        {
            public int Id { get; set; }
            public int Capacity { get; set; }
            public int Day { get; set; }

            public Elective(int id, int capacity, int day)
            {
                Id = id;
                Capacity = capacity;
                Day = day;
            }
        }

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
            var electives = ReadElectives();
            var electiveDays = electives.ToDictionary(
                x => x,
                _ => new Random().Next(firstDay, lastDay + 1));
            WriteElectiveDays(electiveDays);
        }

        private static List<Elective> ReadElectives()
        {
            SqlConnection.Open();

            var electives = new List<Elective>();
            var reader = new NpgsqlCommand(@"SELECT id, capacity, day_of_week
                                                    FROM electives
                                                        JOIN elective_days ON electives.id = elective_days.elective_id
                                                    ORDER BY id",
                SqlConnection).ExecuteReader();
            while (reader.Read())
                electives.Add(new Elective(
                    reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetInt32(2)));
            reader.Close();

            SqlConnection.Close();

            return electives;
        }

        private static void WriteElectiveDays(Dictionary<Elective, int> electiveDays)
        {
            SqlConnection.Open();

            foreach (var (elective, day) in electiveDays)
                new NpgsqlCommand(
                    $@"INSERT INTO elective_days 
                              VALUES ({elective.Id}, {day})",
                    SqlConnection).ExecuteNonQuery();

            SqlConnection.Close();
        }

        private static void GenerateChoices(
            int unpopular = 10,
            int popular = 20,
            double unpopularPoints = 1,
            double regularPoints = 2,
            double popularPoints = 4)
        {
            var students = ReadStudentIds();
            var electives = ReadElectives();

            var propabilities = new double[electives.Count];
            var sum = unpopular * unpopularPoints +
                      (electives.Count - (unpopular + popular)) * regularPoints +
                      popular * popularPoints;
            for (var i = 0; i < unpopular; i++)
                propabilities[i] = unpopularPoints / sum;
            for (var i = unpopular; i < electives.Count - popular; i++)
                propabilities[i] = regularPoints / sum;
            for (var i = electives.Count - popular; i < electives.Count; i++)
                propabilities[i] = popularPoints / sum;

            var studentChoices = new Dictionary<string, int[]>();
            foreach (var student in students)
            {
                var choices = new int[5];
                var days = new int[4];

                for (var i = 0; i < choices.Length; i++)
                {
                    var npChoice = np.random.choice(electives.Count, replace: false, p: propabilities);
                    var choice = Convert.ToInt32(npChoice.ToString());
                    var day = electives[choice].Day - 2;
                    while (choices.Contains(choice) || days[day] == 3)
                    {
                        npChoice = np.random.choice(electives.Count, replace: false, p: propabilities);
                        choice = Convert.ToInt32(npChoice.ToString());
                        day = electives[choice].Day - 2;
                    }

                    choices[i] = choice;
                    days[day]++;
                }

                studentChoices.Add(student, choices);
            }

            WriteSelectedElectives(studentChoices);
        }

        private static List<string> ReadStudentIds()
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
            GenerateChoices();
        }
    }
}
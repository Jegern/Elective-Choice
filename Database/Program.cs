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
            reader.ReadLine();
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
            reader.ReadLine();
            while (!reader.EndOfStream)
                marks.Add(Convert.ToInt32(reader.ReadLine()!.Split(',')[1]));
            return marks;
        }

        private static void WriteStudents(Dictionary<string, List<int>> studentMarks, int numberOfStudents)
        {
            SqlConnection.Open();

            new NpgsqlCommand("TRUNCATE students CASCADE", SqlConnection).ExecuteNonQuery();
            Console.WriteLine("\nТаблица со студентами была удалена");

            for (var i = 0; i < studentMarks.Count; i++)
            {
                if (i % (studentMarks.Count / 100) == 0)
                {
                    ClearCurrentConsoleLine();
                    Console.Write($"Создается новая таблица: [{i} / {studentMarks.Count}]");
                }

                var (key, value) = studentMarks.ElementAt(i);
                new NpgsqlCommand(
                    $@"INSERT INTO students 
                              VALUES ('{key.PadLeft(10, '0')}',
                                      {value.Average()},
                                      'Студент {i + 1}',
                                      {i < numberOfStudents})",
                    SqlConnection).ExecuteNonQuery();
            }

            ClearCurrentConsoleLine();
            Console.Write($"Создается новая таблица: [{studentMarks.Count} / {studentMarks.Count}]");

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

            new NpgsqlCommand("TRUNCATE elective_days", SqlConnection).ExecuteNonQuery();
            Console.WriteLine("\nТаблица со днями элективов была удалена");

            for (var i = 0; i < electiveDays.Count; i++)
            {
                if (i % (electiveDays.Count / 100) == 0)
                {
                    ClearCurrentConsoleLine();
                    Console.Write($"Создается новая таблица: [{i} / {electiveDays.Count}]");
                }
                
                var (elective, day) = electiveDays.ElementAt(i);
                new NpgsqlCommand(
                    $@"INSERT INTO elective_days 
                              VALUES ({elective.Id}, {day})",
                    SqlConnection).ExecuteNonQuery();
            }
            
            ClearCurrentConsoleLine();
            Console.Write($"Создается новая таблица: [{electiveDays.Count} / {electiveDays.Count}]");

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

            new NpgsqlCommand("TRUNCATE selected_electives", SqlConnection).ExecuteNonQuery();
            Console.WriteLine("\nТаблица с выборами студентов была удалена");

            for (var i = 0; i < studentChoices.Count; i++)
            {
                if (i % (studentChoices.Count / 100) == 0)
                {
                    ClearCurrentConsoleLine();
                    Console.Write($"Создается новая таблица: [{i} / {studentChoices.Count}]");
                }
                
                var (student, choices) = studentChoices.ElementAt(i);
                for (var j = 0; j < choices.Length; j++)
                    new NpgsqlCommand(
                        $@"INSERT INTO selected_electives 
                                  VALUES ('{student}', {choices[j]}, {j + 1}, {false})",
                        SqlConnection).ExecuteNonQuery();
            }
            
            ClearCurrentConsoleLine();
            Console.Write($"Создается новая таблица: [{studentChoices.Count} / {studentChoices.Count}]");

            SqlConnection.Close();
        }

        public static void Main(string[] args)
        {
            var work = true;
            while (work)
            {
                Console.Clear();
                Console.Write("1. Сгенерировать N студентов\n" +
                              "2. Сгенерировать выбор студентов\n" +
                              "3. Распределить элективы по дням недели\n" +
                              "4. Выход\n" +
                              "Выберите пункт: ");
                try
                {
                    var line = Console.ReadLine();
                    switch (line)
                    {
                        case "1":
                            Console.Write("\nПри этом будут стерты все выборы элективов. Вы уверены? (Y / N): ");
                            line = Console.ReadLine();
                            if ((line == string.Empty ? "y" : line)?.ToLower() == "y")
                            {
                                Console.Write("Сколько студентов вы хотите сгенерировать? (max = 5084): ");
                                line = Console.ReadLine();
                                var numberOfStudents = Convert.ToInt32(line == string.Empty ? 5084 : line);
                                if (numberOfStudents is > 0 and <= 5084)
                                {
                                    GenerateStudents(numberOfStudents);
                                    Console.WriteLine("\nСтуденты успешно сгенерированы");
                                }
                            }

                            break;
                        case "2":
                            Console.Write("\nСколько элективов будут непопулярны? (std = 10): ");
                            line = Console.ReadLine();
                            var unpopular = Convert.ToInt32(line == string.Empty ? 10 : line);
                            Console.Write("Сколько элективов будут популярны? (std = 20): ");
                            line = Console.ReadLine();
                            var popular = Convert.ToInt32(line == string.Empty ? 20 : line);
                            Console.Write("Сколько очков получат непопулярные элективы? (std = 1.0): ");
                            line = Console.ReadLine();
                            var unpopularPoints = Convert.ToDouble(line == string.Empty ? 1.0 : line);
                            Console.Write("Сколько очков получат обычные элективы? (std = 2.0): ");
                            line = Console.ReadLine();
                            var regularPoints = Convert.ToDouble(line == string.Empty ? 2.0 : line);
                            Console.Write("Сколько очков получат популярные элективы? (std = 4.0): ");
                            line = Console.ReadLine();
                            var popularPoints = Convert.ToDouble(line == string.Empty ? 4.0 : line);
                            if (unpopular is > 0 and <= 220 &&
                                popular is > 0 and <= 220 &&
                                unpopular + popular <= 220 &&
                                unpopularPoints > 0 &&
                                regularPoints > 0 &&
                                popularPoints > 0 &&
                                unpopularPoints <= regularPoints &&
                                unpopularPoints <= popularPoints &&
                                regularPoints <= popularPoints)
                            {
                                GenerateChoices(unpopular, popular, unpopularPoints, regularPoints, popularPoints);
                                Console.WriteLine("\nВыборы студентов успешно сгенерированы");
                            }

                            break;
                        case "3":
                            Console.Write("\nНомер дня недели, который будет первым (std = 2): ");
                            line = Console.ReadLine();
                            var firstDay = Convert.ToInt32(line == string.Empty ? 2 : line);
                            Console.Write("Номер дня недели, который будет последним (std = 5): ");
                            line = Console.ReadLine();
                            var lastDay = Convert.ToInt32(line == string.Empty ? 5 : line);
                            if (firstDay is > 0 and <= 7 && lastDay is > 0 and <= 7 && firstDay <= lastDay)
                            {
                                DistributeElectivesByDay(firstDay, lastDay);
                                Console.WriteLine("\nЭлективы успешно распределены по дням");
                            }

                            break;
                        case "4":
                            work = false;
                            break;
                        case "5":
                            GenerateSomeStrings();
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                if (work)
                    Console.ReadKey();
            }
        }

        private static void GenerateSomeStrings()
        {
            Console.WriteLine("А вот и строки:");
            Console.WriteLine("Один");
            Console.WriteLine("Два");
            Console.Write("Три");
            Thread.Sleep(5000);
            ClearCurrentConsoleLine();
        }

        private static void ClearCurrentConsoleLine()
        {
            var currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.BufferWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }
    }
}
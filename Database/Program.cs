using Npgsql;

namespace Database
{
    public static class InterfaceData
    {
        private static NpgsqlConnection SqlConnection { get; } =
            new("Server=localhost;Port=5432;User Id=postgres;Password=12345;Database=electives;");

        public static void FillDatabaseWithStudents()
        {
            var filePath = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName +
                           "/Student Marks/";
            var xLines = File.ReadAllLines(filePath + "X_train.csv")
                .Select(x => x.Split(';'))
                .ToArray();
            var yLines = File.ReadAllLines(filePath + "y_train.csv")
                .Select(x => x.Split(';'))
                .ToArray();

            var studentMarks = new Dictionary<string, List<int>>();
            for (var i = 1; i < xLines.Length; i++)
            {
                var studentId = xLines[i][0].Split(',')[1];
                var studentMark = Convert.ToInt32(yLines[i][0].Split(',')[1]);
                if (studentMarks.ContainsKey(studentId))
                    studentMarks[studentId].Add(studentMark);
                else
                    studentMarks.Add(studentId, new List<int> {studentMark});
            }

            SqlConnection.Open();

            for (var i = 0; i < studentMarks.Count; i++)
            {
                var (key, value) = studentMarks.ElementAt(i);
                new NpgsqlCommand(
                    $@"INSERT INTO students VALUES ('{key.PadLeft(10, '0')}', {value.Average()}, 'Студент {i}')",
                    SqlConnection).ExecuteNonQuery();
            }

            SqlConnection.Close();
        }

        public static void DistributeElectivesByDay()
        {
            SqlConnection.Open();

            Dictionary<int, int> electiveDays;
            using (var reader = new NpgsqlCommand(@"SELECT id 
                                                           FROM electives 
                                                           ORDER BY id", SqlConnection).ExecuteReader())
            {
                electiveDays = new Dictionary<int, int>();
                while (reader.Read())
                    electiveDays.Add(reader.GetInt32(0), new Random().Next(2, 6));
            }

            foreach (var (key, value) in electiveDays)
                new NpgsqlCommand(
                    $@"INSERT INTO elective_days VALUES ({key}, {value})",
                    SqlConnection).ExecuteNonQuery();

            SqlConnection.Close();
        }

        public static void Main(string[] args)
        {
        }
    }
}
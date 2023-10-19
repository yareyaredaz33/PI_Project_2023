using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace ConsoleApp1
{
    public class SQLiteManager
    {
        private SQLiteConnection _connection;

        public SQLiteManager(string dbPath)
        {
            _connection = new SQLiteConnection($"Data Source={dbPath};Version=3;");
        }

        public void OpenConnection()
        {
            _connection.Open();
        }

        public void CloseConnection()
        {
            _connection.Close();
        }

        public void EliminateData()
        {
            OpenConnection();
            using (var cmd = new SQLiteCommand("DELETE FROM application", _connection))
            {
                cmd.ExecuteNonQuery();
            }

            using (var cmd = new SQLiteCommand("DELETE FROM sessions", _connection))
            {
                cmd.ExecuteNonQuery();
            }
            CloseConnection();
        }
        public void CreateTables()
        {
            OpenConnection();
            using (var command = new SQLiteCommand(_connection))
            {
                
                command.CommandText = @"CREATE TABLE IF NOT EXISTS application
                                        (
                                            app_id INTEGER PRIMARY KEY,
                                            app_name TEXT,
                                            app_category TEXT
                                        )";
                command.ExecuteNonQuery();

                
                command.CommandText = @"CREATE TABLE IF NOT EXISTS sessions
                                        (
                                            app_id INTEGER,
                                            start_time DATETIME,
                                            end_time DATETIME
                                        )";
                command.ExecuteNonQuery();
            }
            CloseConnection();
        }

        public void FillApplicationTableWithRandomData()
        {
            OpenConnection(); 
            using (var insertCmd = new SQLiteCommand("INSERT INTO application (app_id, app_name, app_category) VALUES (@appId, @app_name, @app_category)", _connection))
            {
                var random = new Random();

                for (int i = 0; i < 30; i++) 
                {
                    int newAppId;
                    bool uniqueIdFound = false;

                    do
                    {
                        newAppId = random.Next(50, 1000); 
                        using (var checkCmd = new SQLiteCommand("SELECT COUNT(*) FROM application WHERE app_id = @appId", _connection))
                        {
                            checkCmd.Parameters.AddWithValue("@appId", newAppId);
                            uniqueIdFound = (long)checkCmd.ExecuteScalar() == 0;
                        }
                    } while (!uniqueIdFound);

                    
                    insertCmd.Parameters.AddWithValue("@appId", newAppId);
                    insertCmd.Parameters.AddWithValue("@app_name", GenerateRandomAppName());
                    insertCmd.Parameters.AddWithValue("@app_category", GenerateRandomCategory()); 
                    insertCmd.ExecuteNonQuery();
                    insertCmd.Parameters.Clear();
                }
            }
            CloseConnection(); 
        }
        public string GenerateRandomCategory()
        {
            string[] categories = new string[]
            {
        "fire", "conscience", "garage", "lodge", "jelly", "grimace", "file", "brilliance", "love",
        "revolution", "cooperate", "counter", "tick", "interface", "courage", "soldier", "avant-garde",
        "fate", "harass", "crime", "swim", "crowd", "referral",
        "clique", "ghostwriter", "hear", "banana", "excitement", "convenience",
        "curriculum", "week", "oral", "steak", "colorful"
            };

            Random random = new Random();
            return categories[random.Next(categories.Length)];
        }

        public string GenerateRandomAppName()
        {
            List<string> appNames = new List<string>
            {
        "DataForge Pro", "SwiftCrest", "CodePulse", "LogicLink", "CyberSphere", "TechCraft", "MindByte",
        "Infinitum Suite", "SynthWave", "CodeNinja", "QuantumConnect", "ByteHive", "TechSprint", "NexusFlow", "DataZen"
            };

            Random random = new Random();
            return appNames[random.Next(appNames.Count)];
        }

        /*   static string GenerateRandomWord(int length)
           {
               const string chars = "abcdefghijklmnopqrstuvwxyz";
               Random random = new Random();
               char[] wordChars = new char[length];

               for (int i = 0; i < length; i++)
               {
                   int index = random.Next(0, chars.Length);
                   wordChars[i] = chars[index];
               }

               return new string(wordChars);
           }*/
        public void FillSessionsTableWithRandomData()
        {
            OpenConnection(); 
            using (var insertCmd = new SQLiteCommand("INSERT INTO sessions (app_id, start_time, end_time) VALUES (@appId, @start_time, @end_time)", _connection))
            {
                var random = new Random();

                
                DateTime startTime = DateTime.Now;
                DateTime endTime = startTime.AddHours(random.Next(1, 5));

                for (int i = 0; i < 30; i++) 
                {
                    insertCmd.Parameters.AddWithValue("@appId", random.Next(50, 1000)); // Generate a random app_id
                    insertCmd.Parameters.AddWithValue("@start_time", startTime);
                    insertCmd.Parameters.AddWithValue("@end_time", endTime);
                    insertCmd.ExecuteNonQuery();
                    insertCmd.Parameters.Clear();

                    
                    startTime = endTime.AddHours(random.Next(1, 5));
                    endTime = startTime.AddHours(random.Next(1, 5));
                }
            }
            CloseConnection(); 
        }
        public void PrintApplicationData()
        {
            OpenConnection();
            using (var command = new SQLiteCommand("SELECT * FROM application", _connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int appId = reader.GetInt32(0);
                    string appName = reader.GetString(1);
                    string appCategory = reader.GetString(2);

                    
                    string output = $"{appId} : {appName} from the category {appCategory}";
                    Console.WriteLine(output);
                }
            }
            CloseConnection();
        }
        public void PrintSessionsData()
        {
            OpenConnection();
            using (var command = new SQLiteCommand("SELECT * FROM sessions", _connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int appId = reader.GetInt32(0);
                    DateTime startTime = reader.GetDateTime(1);
                    DateTime endTime = reader.GetDateTime(2);

                    
                    string output = $"App ID: {appId}, Start Time: {startTime}, End Time: {endTime}, use time: {endTime-startTime}";
                    Console.WriteLine(output);
                }
            }
            CloseConnection(); 
        }

    }
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string dbPath = "your_db_path.db"; 
                SQLiteManager manager = new SQLiteManager(dbPath);
                manager.CreateTables();
                manager.EliminateData();
                manager.FillApplicationTableWithRandomData();
                manager.FillSessionsTableWithRandomData();


                manager.PrintApplicationData();
                manager.PrintSessionsData();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}

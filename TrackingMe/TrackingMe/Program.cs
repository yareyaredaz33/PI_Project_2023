using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var connString = "Host=localhost;Username=postgres;Password=1111;Database=TrackMe";

            
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                EliminateData(conn);
                FillApplicationTableWithRandomData(conn);
                using (var selectCmd = new NpgsqlCommand("SELECT * FROM application", conn))
                using (var reader = selectCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int appId = reader.GetInt32(0);
                        string appName = reader.GetString(1);
                        string appCategory = reader.GetString(2);

                        // Concatenate the values and print them
                        string output = $"{appId} : {appName} from the category {appCategory}";
                        Console.WriteLine(output);
                    }
                }

            }

            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();
        }
        
        static void FillApplicationTableWithRandomData(NpgsqlConnection conn)
        {
            using (var insertCmd = new NpgsqlCommand("INSERT INTO application (app_id, app_name, app_category) VALUES (@appId, @app_name, @app_category)", conn))
            {
                var random = new Random();

                for (int i = 0; i < 30; i++) // Додати 30 записів
                {
                    insertCmd.Parameters.AddWithValue("@appId", random.Next(50, 1000)); // Генеруємо випадковий app_id
                    insertCmd.Parameters.AddWithValue("@app_name", GenerateRandomWord(random.Next(15, 20)));
                    insertCmd.Parameters.AddWithValue("@app_category", GenerateRandomWord(random.Next(15,30))); // Генеруємо інші випадкові дані
                    insertCmd.ExecuteNonQuery();
                    insertCmd.Parameters.Clear();
                }
            }
        }
        static void EliminateData(NpgsqlConnection conn)
        {
            using (var cmd = new NpgsqlCommand("DELETE FROM application", conn))
            {
                cmd.ExecuteNonQuery();
            }

            using (var cmd = new NpgsqlCommand("DELETE FROM sessions", conn))
            {
                cmd.ExecuteNonQuery();
            }

        }
        static string GenerateRandomWord(int length)
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
        }
    }
}

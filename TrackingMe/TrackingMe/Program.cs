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


                using (var selectCmd = new NpgsqlCommand("SELECT app_id FROM application", conn))
                using (var reader = selectCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Read the integer value and convert it to a string
                        int intValue = reader.GetInt32(0);
                        string valueAsString = intValue.ToString();
                        Console.WriteLine(valueAsString);
                    }

                }

            }
            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();
        }
    }
}

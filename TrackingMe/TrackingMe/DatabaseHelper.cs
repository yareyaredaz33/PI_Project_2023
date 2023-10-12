using System;
using System.Data;
using Npgsql;

class DatabaseHelper
{
    private string connectionString; // Рядок підключення до бази даних

    static void FillApplicationTableWithRandomData(NpgsqlConnection conn)
    {
        using (var insertCmd = new NpgsqlCommand("INSERT INTO application (app_id, app_name, app_category) VALUES (@appId, @app_name, @app_category)", conn))
        {
            var random = new Random();

            for (int i = 0; i < 30; i++) // Додати 30 записів
            {
                insertCmd.Parameters.AddWithValue("@appId", random.Next(1, 1000)); // Генеруємо випадковий app_id
                insertCmd.Parameters.AddWithValue("@app_name", GenerateRandomWord(random.Next(3, 35)));
                insertCmd.Parameters.AddWithValue("@app_category", GenerateRandomWord(random.Next(50, 55))); // Генеруємо інші випадкові дані
                insertCmd.ExecuteNonQuery();
                insertCmd.Parameters.Clear();
            }
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

using System;
using System.Data;
using System.Data.SqlClient;

class DatabaseHelper
{
    private string connectionString; // Рядок підключення до бази даних

    public DatabaseHelper(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public void PrintDataFromTable(string tableName)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            using (SqlCommand command = new SqlCommand($"SELECT * FROM {tableName}", connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["ColumnName1"]} - {reader["ColumnName2"]}"); // Замініть на імена колонок з вашої таблиці
                    }
                }
            }
        }
    }

    public void PopulateTestData()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            // Додайте ваші SQL запити для створення та заповнення таблиць тестовими даними
            string createTableQuery = "CREATE TABLE TableName (ColumnName1 datatype, ColumnName2 datatype)";
            string insertDataQuery = "INSERT INTO TableName (ColumnName1, ColumnName2) VALUES (@Value1, @Value2)";

            using (SqlCommand command = new SqlCommand(createTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }

            // Заповнення даними
            for (int i = 0; i < 50; i++)
            {
                using (SqlCommand command = new SqlCommand(insertDataQuery, connection))
                {
                    command.Parameters.AddWithValue("@Value1", $"Value{i}");
                    command.Parameters.AddWithValue("@Value2", i);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}

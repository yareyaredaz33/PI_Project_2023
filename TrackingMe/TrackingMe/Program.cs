using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TrackingMe
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Your Connection String"; // Замініть на свій рядок підключення
            DatabaseHelper dbHelper = new DatabaseHelper(connectionString);

            dbHelper.PopulateTestData(); // Заповнити тестовими даними
            dbHelper.PrintDataFromTable("TableName"); // Друк даних з таблиці
        }
    }
}

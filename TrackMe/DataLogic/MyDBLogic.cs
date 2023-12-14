// TrackMe.DAL/MyDbContext.cs
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using TrackMe;
using System.Diagnostics;
using System.Data;

namespace TrackMe.DataLogic
{
    public class MyDBLogic : DbContext
    {
        private static MyDBLogic dbLogicInstance;
        private readonly string _databaseName = "ProgramsInfo.db";

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("your_sqlite_connection_string_here");
        }

        private SQLiteConnection _connection;
        private readonly string _programPath;

        public MyDBLogic(string programPath)
        {
            var solutionPath = Directory.GetParent(programPath).Parent.Parent.FullName;
            var dbFileName = _databaseName; // Specify your database filename
            var dbPath = Path.Combine(solutionPath, dbFileName);
            _connection = new SQLiteConnection($"Data Source={_databaseName};Version=3;");

            // Use the specified database name when creating tables
            CreateDatabase(_databaseName);
            CreateTables();
        }

        private void CreateDatabase(string databaseName)
        {
            try
            {
                _connection = new SQLiteConnection($"Data Source={databaseName};Version=3;");

                if (!File.Exists(_connection.DataSource))
                {
                    SQLiteConnection.CreateFile(_connection.DataSource);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error creating database: {ex.Message}");
            }
        }

        private void CreateTables()
        {
            try
            {
                OpenConnection();

                using (var command = new SQLiteCommand(_connection))
                {
                    // Create your tables here in the specified database
                    command.CommandText = $@"CREATE TABLE IF NOT EXISTS Users
                                (
                                    Id INTEGER PRIMARY KEY,
                                    Name TEXT
                                    -- Add other columns as needed
                                )";

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error creating tables: {ex.Message}");
            }
            finally
            {
                CloseConnection();
            }
        }
        public void OpenConnection()
        {
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }
        }

        public void CloseConnection()
        {
            if (_connection.State != ConnectionState.Closed)
            {
                _connection.Close();
            }
        }

        public static void SetInstance(MyDBLogic instance)
        {
            dbLogicInstance = instance;
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




        public void PrintTableNames()
        {
            OpenConnection();
            try
            {
                using (var command = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type='table'", _connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string tableName = reader.GetString(0);
                        Console.WriteLine($"Table Name: {tableName}");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error printing table names: {ex.Message}");
            }
            finally
            {
                CloseConnection();
            }
        }



        public void AddProcessInfoToDatabase(string processName, int processId)
        {
            try
            {
                // Check if the table exists, if not, create it in the ProgramsInfo database
                if (!TableExists("ProgramsInfo"))
                {
                    CreateTableForProgramsInfo();
                    Debug.WriteLine("Table ProgramsInfo created.");
                }

                // Add a new row to the ProgramsInfo table with start time
                InsertProcessStart("ProgramsInfo", processId, processName, DateTime.Now);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error adding process info to the database: {ex.Message}");
            }
        }

        public void CreateTableForProgramsInfo()
        {
            OpenConnection();
            using (var command = new SQLiteCommand(_connection))
            {
                command.CommandText = @"CREATE TABLE IF NOT EXISTS ProgramsInfo
                            (
                                entry_id INTEGER PRIMARY KEY NOT NULL,
                                start_time DATETIME NOT NULL,
                                end_time DATETIME,
                                process_id INTEGER,
                                process_name TEXT
                                -- Add other columns as needed
                            )";
                command.ExecuteNonQuery();
            }
            CloseConnection();
        }
        public List<string> GetTableNames()
        {
            OpenConnection();
            var tableNames = new List<string>();
            try
            {
                using (var command = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type='table'", _connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string tableName = reader.GetString(0);
                        tableNames.Add(tableName);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error retrieving table names: {ex.Message}");
            }
            finally
            {
                CloseConnection();
            }
            return tableNames;
        }
        public void InsertProcessStart(string tableName, int processId, string processName, DateTime startTime)
        {
            OpenConnection();
            using (var command = new SQLiteCommand(_connection))
            {
                command.CommandText = $@"INSERT INTO {tableName} (start_time, end_time, process_id, process_name)
                              VALUES (@startTime, NULL, @processId, @processName)";
                command.Parameters.AddWithValue("@startTime", startTime);
                command.Parameters.AddWithValue("@processId", processId);
                command.Parameters.AddWithValue("@processName", processName);
                command.ExecuteNonQuery();
            }
            CloseConnection();
        }

        private bool TableExists(string tableName)
        {
            OpenConnection();

            using (var command = new SQLiteCommand($"SELECT name FROM sqlite_master WHERE type='table' AND name='{tableName}'", _connection))
            using (var reader = command.ExecuteReader())
            {
                bool exists = reader.HasRows;
                CloseConnection();
                return exists;
            }
        }
    }




        public class User
        {
        public int Id { get; set; }
        public string Name { get; set; }
        // Додай інші властивості користувача за потребою
        }
}

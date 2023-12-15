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
using System.Globalization;

namespace TrackMe.DataLogic
{
    public class MyDBLogic : DbContext
    {

        private SQLiteConnection _connection;
        private readonly string _databaseName = "ProgramsInfo.db";
        private static int _uniqueIdCounter = 1;

        public DbSet<User> Users { get; set; }

        public MyDBLogic(string programPath)
        {
            var solutionPath = Directory.GetParent(programPath).Parent.Parent.FullName;


            _connection = new SQLiteConnection($"Data Source={programPath};Version=3;");

            // Use the specified database name when creating tables
            CreateDatabase(programPath);
            Debug.WriteLine("created and connected succesfuly\n\n\n\n");

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

        public void CreateTable(string ProgramName)
        {
            string sanitizedProgramName = SanitizeTableName(ProgramName);
            OpenConnection();
            try
            {
                using (var command = new SQLiteCommand(_connection))
                {
                    // Sanitize the table name by replacing invalid characters with underscores

                    // Create your tables here in the specified database
                    command.CommandText = $@"CREATE TABLE IF NOT EXISTS {sanitizedProgramName}
                    (
                        Id INTEGER PRIMARY KEY,
                        Name TEXT,
                        StartTime DATETIME,
                        EndTime DATETIME
                    )";

                    command.ExecuteNonQuery();
                    Debug.WriteLine("created TABLE\n\n\n\n");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error creating table {sanitizedProgramName}: {ex.Message}");
                // Optionally, you can throw the exception to stop the execution or log the error.
            }
            finally
            {
                CloseConnection();
            }
        }

            public string SanitizeTableName(string tableName)
        {
            // Replace invalid characters with underscores
            foreach (char invalidChar in Path.GetInvalidFileNameChars())
            {
                tableName = tableName.Replace(invalidChar, '_');
            }
            return tableName;
        }

        public void FillTable(string ProgramName)
        {
            try
            {
                OpenConnection();

                // Get the last assigned ID from the table
                int lastAssignedId = GetLastAssignedId(ProgramName);

                // Check if the table is empty
                if (lastAssignedId == -1)
                {
                    // Table is empty, start from ID 1
                    lastAssignedId = 0;
                }

                // Increment to get the new unique ID
                int generatedId = lastAssignedId + 1;

                DateTime startTime = DateTime.Now;
                DateTime? endTime = null; // Set it to null or some default value, depending on your requirements

                using (var insertCmd = new SQLiteCommand($"INSERT INTO {ProgramName} (Id, Name, StartTime, EndTime) VALUES (@Id, @Name, @StartTime, @EndTime)", _connection))
                {
                    insertCmd.Parameters.AddWithValue("@Id", generatedId);
                    insertCmd.Parameters.AddWithValue("@Name", ProgramName);
                    insertCmd.Parameters.AddWithValue("@StartTime", startTime);
                    insertCmd.Parameters.AddWithValue("@EndTime", endTime);

                    insertCmd.ExecuteNonQuery();
                }
                Debug.WriteLine("filled\n");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error filling table {ProgramName}: {ex.Message}");
                // Optionally, you can throw the exception to stop the execution or log the error.
            }
            finally
            {
                CloseConnection();
            }
        }

        public void DeleteAllRows()
        {
            try
            {
                OpenConnection(); // Ensure the connection is open

                // Disable foreign key constraints
                using (var pragmaCommand = new SQLiteCommand("PRAGMA foreign_keys=off", _connection))
                {
                    pragmaCommand.ExecuteNonQuery();
                }

                List<string> tableNames = GetTableNames();

                foreach (var tableName in tableNames)
                {
                    // Ensure the connection is open before executing the command
                    if (_connection.State != ConnectionState.Open)
                    {
                        _connection.Open();
                    }

                    using (var command = new SQLiteCommand($"DELETE FROM {tableName}", _connection))
                    {
                        command.ExecuteNonQuery();
                        Debug.WriteLine($"Deleted all rows from table {tableName}");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deleting rows: {ex.Message}");
            }
            finally
            {
                // Enable foreign key constraints
                using (var pragmaCommand = new SQLiteCommand("PRAGMA foreign_keys=on", _connection))
                {
                    pragmaCommand.ExecuteNonQuery();
                }

                CloseConnection(); // Ensure to close the connection after use
            }
        }







        private int GetLastAssignedId(string tableName)
        {
            try
            {
                using (var command = new SQLiteCommand($"SELECT MAX(Id) FROM {tableName}", _connection))
                {
                    var result = command.ExecuteScalar();
                    return result == DBNull.Value ? -1 : Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting last assigned ID for table {tableName}: {ex.Message}");
                // Optionally, you can throw the exception to stop the execution or log the error.
                return -1; // or another suitable value indicating an error
            }
        }

        private int GenerateUniqueId()
        {
            // Increment the counter and return the new value
            return _uniqueIdCounter++;
        }

        public void UpdateLastRowEndTime(string ProgramName)
        {
            OpenConnection();
            try
            {
                // Get the last row where EndTime is null
                using (var command = new SQLiteCommand($"SELECT * FROM {ProgramName} WHERE EndTime IS NULL ORDER BY Id DESC LIMIT 1", _connection))
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int id = reader.GetInt32(reader.GetOrdinal("Id"));
                        DateTime endTime = DateTime.Now;

                        // Update the EndTime for the found row
                        using (var updateCmd = new SQLiteCommand($"UPDATE {ProgramName} SET EndTime = @EndTime WHERE Id = @Id", _connection))
                        {
                            updateCmd.Parameters.AddWithValue("@Id", id);
                            updateCmd.Parameters.AddWithValue("@EndTime", endTime);
                            updateCmd.ExecuteNonQuery();
                        }

                        Debug.WriteLine($"Updated EndTime for the last row in table {ProgramName}");
                    }
                    else
                    {
                        Debug.WriteLine($"No rows with EndTime=null found in table {ProgramName}");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating EndTime: {ex.Message}");
            }
            finally
            {
                CloseConnection();
            }
        }

        public void DeleteAllTables()
        {
            try
            {
                OpenConnection(); // Ensure the connection is open

                // Disable foreign key constraints
                using (var pragmaCommand = new SQLiteCommand("PRAGMA foreign_keys=off", _connection))
                {
                    pragmaCommand.ExecuteNonQuery();
                }

                List<string> tableNames = GetTableNames();

                foreach (var tableName in tableNames)
                {
                    using (var command = new SQLiteCommand($"DROP TABLE IF EXISTS {tableName}", _connection))
                    {
                        command.ExecuteNonQuery();
                        Debug.WriteLine($"Table {tableName} deleted");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deleting tables: {ex.Message}");
            }
            finally
            {


                CloseConnection(); // Ensure to close the connection after use
            }
        }


        public void UpdateAllTablesLastRowEndTime()
        {
            OpenConnection();
            try
            {
                List<string> tableNames = GetTableNames();

                foreach (var tableName in tableNames)
                {
                    UpdateLastRowEndTime(tableName);
                }
                Debug.WriteLine("updatelastertow");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating EndTime in all tables: {ex.Message}");
            }
            finally
            {
                CloseConnection();
            }
        }

        public void CalculateAndPrintTotalTimeSpent(string timeLapse)
        {
            OpenConnection();
            try
            {
                List<string> tableNames = GetTableNames();

                foreach (var tableName in tableNames)
                {
                    TimeSpan totalTime = CalculateTotalTime(tableName, timeLapse);

                    // Skip printing tables with zero total time
                    if (totalTime != TimeSpan.Zero)
                    {
                        Debug.WriteLine($"Table: {tableName}, Total Time Spent: {FormatTime(totalTime)}");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error calculating and printing total time: {ex.Message}");
            }
            finally
            {
                CloseConnection();
            }
        }

        private TimeSpan CalculateTotalTime(string tableName, string timeLapse)
        {
            TimeSpan totalTime = TimeSpan.Zero;

            try
            {
                OpenConnection(); // Ensure the connection is open

                DateTime filterDate;

                // Set the filter date based on the time lapse
                switch (timeLapse.ToLower())
                {
                    case "day":
                        filterDate = DateTime.Now.AddDays(-1);
                        break;
                    case "week":
                        filterDate = DateTime.Now.AddDays(-7);
                        break;
                    case "year":
                        filterDate = DateTime.Now.AddYears(-1);
                        break;
                    default:
                        filterDate = DateTime.MinValue; // No filter
                        break;
                }

                using (var command = new SQLiteCommand($"SELECT StartTime, EndTime FROM {tableName}", _connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DateTime startTime = reader.GetDateTime(reader.GetOrdinal("StartTime"));
                        DateTime? endTime = reader["EndTime"] as DateTime?;

                        // Apply the filter based on the time lapse
                        if (filterDate == DateTime.MinValue || (startTime >= filterDate && (endTime == null || endTime >= filterDate)))
                        {
                            // If EndTime is not null, calculate the time difference
                            if (endTime.HasValue)
                            {
                                totalTime += endTime.Value.Subtract(startTime);
                            }
                            else
                            {
                                // If EndTime is null, use DateTime.Now as the end time
                                totalTime += DateTime.Now.Subtract(startTime);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error calculating total time for table {tableName}: {ex.Message}");
            }
            finally
            {
                CloseConnection(); // Ensure to close the connection after use
            }

            return totalTime.TotalMilliseconds > 0 ? totalTime : TimeSpan.Zero;
        }
        private string FormatTime(TimeSpan time)
        {
            return $"{time.Days} days, {time.Hours} hours, {time.Minutes} minutes, {time.Seconds} seconds, and {time.Milliseconds} milliseconds";
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



    }




    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        // Додай інші властивості користувача за потребою
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Windows;

namespace TrackMe
{
    public partial class MainWindow : Window
    {
        private TrackMe.DataLogic.MyDBLogic dbLogicInstance;



        public MainWindow()
        {
            dbLogicInstance = new TrackMe.DataLogic.MyDBLogic(AppDomain.CurrentDomain.BaseDirectory);
            TrackMe.DataLogic.MyDBLogic.SetInstance(dbLogicInstance);  // Pass the instance
            InitializeComponent();
            GetOpenApplications();
            dbLogicInstance.PrintTableNames(); // Call the method to print table names
        }

        private void GetOpenApplications()
        {
            try
            {
                var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Process WHERE ExecutablePath IS NOT NULL");
                var processes = searcher.Get().Cast<ManagementObject>().ToList();

                // Use a set to keep track of unique process names
                var uniqueProcessNames = new HashSet<string>();

                foreach (var process in processes)
                {
                    var processName = process["Name"].ToString();
                    var processId = Convert.ToInt32(process["ProcessId"]);
                    var executablePath = process["ExecutablePath"]?.ToString();

                    // Check if the process is a duplicate, a known system process, or a background process
                    if (!uniqueProcessNames.Contains(processName) &&
                        !IsSystemProcess(processName, executablePath) &&
                        HasVisibleWindow(processId))
                    {
                        // Add the process name to the set to track duplicates
                        uniqueProcessNames.Add(processName);

                        // Determine if the process is an App or Background Process
                        var processType = IsAppProcess(processName) ? "App" : "Background Process";

                        // Create a ProcessInfo object
                        var processInfo = new ProcessInfo { ProcessName = processName, ProcessId = processId, ProcessType = processType };

                        // Add the process information to your ListView
                        listView.Items.Add(processInfo);

                        dbLogicInstance.AddProcessInfoToDatabase(processInfo.ProcessName, processInfo.ProcessId);

                        // Print application information to the ListView
                        listView.Items.Add(processInfo);

                        // Print table names to the TextBox
                        var tableNames = dbLogicInstance.GetTableNames();
                        tableNameTextBox.AppendText("Table Names:\n");
                        foreach (var tableName in tableNames)
                        {
                            tableNameTextBox.AppendText($"{tableName}\n");
                        }
                        tableNameTextBox.AppendText("\nTest Message");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool HasVisibleWindow(int processId)
        {
            try
            {
                Process[] processes = Process.GetProcesses();

                foreach (Process process in processes)
                {
                    if (process.Id == processId)
                    {
                        // Check if the process has a taskbar entry (non-empty window title)
                        return !string.IsNullOrEmpty(process.MainWindowTitle);
                    }
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        private bool IsAppProcess(string processName)
        {
            // Additional criteria to identify Apps
            return !processName.ToLower().Contains("background") &&
                   !processName.ToLower().Contains("service") &&
                   !processName.ToLower().Contains("host") &&
                   !processName.ToLower().Contains("system");
        }

        private bool IsSystemProcess(string processName, string executablePath)
        {
            // Add more criteria here to identify system processes
            return processName.ToLower().Contains("system") ||
                   processName.ToLower().Contains("services") ||
                   (executablePath != null && executablePath.ToLower().Contains("host") && !executablePath.ToLower().Contains("system32"));
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        // Define a class to store process information
        private class ProcessInfo
        {
            public string ProcessName { get; set; }
            public int ProcessId { get; set; }
            public string ProcessType { get; set; }
        }
    }
}
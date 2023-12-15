using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Threading;
using System.IO;
namespace TrackMe
{
    public partial class MainWindow : Window
    {
        private List<ProcessInfo> oldProcessInfo;
        private List<ProcessInfo> newProcessInfo;
        private TrackMe.DataLogic.MyDBLogic dbLogicInstance;
        private List<ProcessInfo> runningProcesses;
        private string[] previousSanitizedProcessNamesArray;

        private string[] sanitizedProcessNamesArray;

        private ManagementEventWatcher processStartWatcher;
        private ManagementEventWatcher processStopWatcher;
        private DispatcherTimer updateTimer;
        

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;

            // Subscribe to the Loaded event
        }



        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Closing += MainWindow_Closing;
            dbLogicInstance = new TrackMe.DataLogic.MyDBLogic("ProgramInfo.db");

            
            // This method will be called when the window is loaded and ready
            runningProcesses = new List<ProcessInfo>();
            // Execute the logic to get open applications and print to the console
            GetOpenApplications();
            
            Debug.WriteLine("\n\n\n\n\n\n\n\n");
            Debug.WriteLine("\n\n\n\n\n\n\n\n");
            Debug.WriteLine("NOWNOWNOWNOW\n");
           
           
            // Print each element of the array to the console
            foreach (var processName in sanitizedProcessNamesArray)
            {
                Debug.WriteLine(processName);
                dbLogicInstance.CreateTable(processName);
                dbLogicInstance.FillTable(processName);

            }
            updateTimer = new DispatcherTimer();
            updateTimer.Interval = TimeSpan.FromSeconds(1);
            updateTimer.Tick += UpdateTimer_Tick;
            updateTimer.Start();

        }


        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            // Call GetOpenApplications to get the current state
            GetOpenApplications();

            // Compare with the previous state
            if (!AreArraysEqual(sanitizedProcessNamesArray, previousSanitizedProcessNamesArray))
            {
                // If the arrays are not equal, update the UI
                UpdateUI();
            }

            // Update the previous state
            previousSanitizedProcessNamesArray = sanitizedProcessNamesArray.ToArray();
        }
        private bool AreArraysEqual(string[] array1, string[] array2)
        {
            if (array1 == null || array2 == null)
            {
                return false;
            }

            if (array1.Length != array2.Length)
            {
                return false;
            }

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                {
                    return false;
                }
            }

            return true;
        }

        private List<string> addedProcesses = new List<string>();
        private List<string> removedProcesses = new List<string>();

        private List<string> previousProcesses = new List<string>();

        private void UpdateUI()
        {
            // Use Dispatcher to update UI on the UI thread
            Dispatcher.Invoke(() =>
            {
                Debug.WriteLine("UI Updated:");

                // Log added and removed processes
                var addedProcesses = sanitizedProcessNamesArray.Except(previousProcesses).ToList();
                var removedProcesses = previousProcesses.Except(sanitizedProcessNamesArray).ToList();

                Debug.WriteLine("Added processes:");
                foreach (var addedProcess in addedProcesses)
                {
                    Debug.WriteLine($"- {addedProcess}");
                    dbLogicInstance.CreateTable(addedProcess);
                    dbLogicInstance.FillTable(addedProcess);
                }

                Debug.WriteLine("Removed processes:");
                foreach (var removedProcess in removedProcesses)
                {
                    Debug.WriteLine($"- {removedProcess}");
                    dbLogicInstance.UpdateLastRowEndTime(removedProcess);
                }

                // Update the previousProcesses list
                previousProcesses = sanitizedProcessNamesArray.ToList();
            });
        }

        private void GetOpenApplications()
        {
            Debug.WriteLine("called");
            dbLogicInstance = new TrackMe.DataLogic.MyDBLogic("ProgramInfo.db");
            List<string> sanitizedProcessNames = new List<string>();

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

                        // Create a new ProcessInfo object
                        var processInfo = new ProcessInfo { ProcessName = processName, ProcessId = processId, ProcessType = processType };

                        // Add the sanitized process name to the list
                        var sanitizedName = dbLogicInstance.SanitizeTableName(processInfo.ProcessName.ToString());

                        // Remove .exe extension from the process name
                        sanitizedName = Path.GetFileNameWithoutExtension(sanitizedName);

                        sanitizedProcessNames.Add(sanitizedName);
                    }
                }

                // If needed, convert the list to an array
                sanitizedProcessNamesArray = sanitizedProcessNames.ToArray();
                // Now, you can use sanitizedProcessNamesArray for further processing or storage.

                // Call UpdateUI to log the changes
                UpdateUI();
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

        public void GetInfo(string timelaps)
        {
            dbLogicInstance.CalculateAndPrintTotalTimeSpent("timelaps");
        }
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // This method will be called when the window is closing
            Debug.WriteLine("MainWindow_Closing is being called");
            updateTimer?.Stop();
            //dbLogicInstance.DeleteAllRows();
            dbLogicInstance.UpdateAllTablesLastRowEndTime();
            dbLogicInstance.CalculateAndPrintTotalTimeSpent("day");

            // Close the database connection (if it is not null)
            dbLogicInstance?.CloseConnection();

            // If needed, you can add more cleanup steps based on your application's requirements.
        }


        // Define a class to store process information
        private class ProcessInfo
        {
            public string ProcessName { get; set; }
            public int ProcessId { get; set; }
            public string ProcessType { get; set; }
        }
    }
}
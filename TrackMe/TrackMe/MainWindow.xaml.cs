using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using Hardcodet.Wpf.TaskbarNotification;
namespace TrackMe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TaskbarIcon notifyIcon;
        public MainWindow()
        {
            InitializeComponent();
            /*GetOpenedPrograms();

            // Initialize NotifyIcon "C:\github\PI_Project_2023\TrackMe\icons\TrackMe.ico"
            notifyIcon = new TaskbarIcon();
            *//*            notifyIcon.IconSource = new BitmapImage(new Uri("pack://application:,,,/TrackMe;component/icons/TrackMe.ico"));
             *            
            *//*
            // Get the directory where the executable is located
            string executablePath = @"C:\github\PI_Project_2023\TrackMe";


            // Combine the directory path with the relative path to your icon
            string iconPath = System.IO.Path.Combine(executablePath, "icons", "TrackMe.ico");

            // Set the icon using the constructed path
            notifyIcon.IconSource = new BitmapImage(new Uri(iconPath));


            notifyIcon.Visibility = Visibility.Collapsed;

            // Add a context menu with an option to restore the window
            var exitMenuItem = new System.Windows.Controls.MenuItem();
            exitMenuItem.Header = "Exit";
            exitMenuItem.Click += ExitMenuItem_Click;

            notifyIcon.ContextMenu = new System.Windows.Controls.ContextMenu();
            notifyIcon.ContextMenu.Items.Add(exitMenuItem);

            // Handle the Closing event to minimize to tray
            Closing += MainWindow_Closing;*/
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new UserInterface.MainPage());
            button.Visibility = Visibility.Collapsed;
            TrackMelogo.Visibility = Visibility.Collapsed;

        }


        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            // Ховаємо вікно в трей
            Visibility = Visibility.Hidden;

            // Показуємо значок в треї
            notifyIcon.Visibility = Visibility.Visible;
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Cancel the default close behavior
            e.Cancel = true;

            // Hide the window
            Visibility = Visibility.Hidden;

            // Show the system tray icon
            notifyIcon.Visibility = Visibility.Visible;
        }
        private void RestoreMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Restore the window when the "Restore" menu item is clicked
            Visibility = Visibility.Visible;

            // Hide the system tray icon
            notifyIcon.Visibility = Visibility.Collapsed;
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Clean up resources and close the application when the "Exit" menu item is clicked
            notifyIcon.Visibility = Visibility.Collapsed;
            notifyIcon.Dispose();
            Close();
        }

        private void GetOpenedPrograms()
        {
            // Replace the following lines with your actual code for getting opened programs
            Process[] processes = Process.GetProcesses();

            foreach (Process process in processes)
            {
                Console.WriteLine("Process Name: " + process.ProcessName);
                // Add your logic to handle each opened program/process
            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            // Ensure the icon is disposed when the application is closing
/*            notifyIcon.Dispose();
            base.OnClosing(e);*/
        }

        private void mainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            // Handle navigation event if needed
        }
    }
}

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
using TrackMe.DataLogic;

namespace UserInterface
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
   
    public partial class MainPage : Page
    { private MyDBLogic? _dbLogic;

        public MainPage()
        {
            InitializeComponent();
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
             NavigationService.Navigate(new UserInterface.Profile());
        }
        private bool isDarkMode = false;


        private void DarkModeToggle_Checked(object sender, RoutedEventArgs e)
        {
            isDarkMode = true;
            ApplyDarkMode();
        }

        private void DarkModeToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            isDarkMode = false;
            ApplyDarkMode();
        }

        private void ApplyDarkMode()
        {
            if (isDarkMode)
            {
                Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("/YourApp;component/Styles/DarkStyles.xaml", UriKind.RelativeOrAbsolute) });
            }
            else
            {
                // Remove the dark styles
                var darkStyles = Resources.MergedDictionaries.FirstOrDefault(rd => rd.Source != null && rd.Source.OriginalString.EndsWith("DarkStyles.xaml"));
                if (darkStyles != null)
                {
                    Resources.MergedDictionaries.Remove(darkStyles);
                }
            }
        }
        private void LoadData()
        {
            try
            {
                List<string> tableNames = _dbLogic.GetTableNames();

<<<<<<< Updated upstream
        private void BackgrondMode(object sender, RoutedEventArgs e)
        {

            Window parentWindow = Window.GetWindow(this);

            if (parentWindow != null)
            {
                parentWindow.WindowState = WindowState.Minimized;
            }
        }

=======
                foreach (var tableName in tableNames)
                {
                    // Assuming you have a ListView named "dataListView" in your XAML
                    dataListView.Items.Add(new { TableName = tableName });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
>>>>>>> Stashed changes
        private void Button_Click_2(object sender, RoutedEventArgs e)

        {
            Application.Current.Shutdown();
        }
    }
    public class ProgramViewModel
    {
        public string TableName { get; set; }
        public TimeSpan TotalTimeSpent { get; set; }
    }
}

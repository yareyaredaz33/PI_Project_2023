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

namespace UserInterface
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
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

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}

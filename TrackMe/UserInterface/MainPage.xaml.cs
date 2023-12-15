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


       
   
        private void BackgrondMode(object sender, RoutedEventArgs e)
        {

            Window parentWindow = Window.GetWindow(this);

            if (parentWindow != null)
            {
                parentWindow.WindowState = WindowState.Minimized;
            }
        }


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

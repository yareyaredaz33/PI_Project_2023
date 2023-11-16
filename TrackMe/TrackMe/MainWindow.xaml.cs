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

namespace TrackMe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            //InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new Registration1());
            button.Visibility = Visibility.Collapsed;
            TrackMelogo.Visibility = Visibility.Collapsed;



        }

        private void mainFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }
    }
}

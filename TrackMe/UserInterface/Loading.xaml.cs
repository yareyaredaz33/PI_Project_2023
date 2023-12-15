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
    /// Interaction logic for Loading.xaml
    /// </summary>
    public partial class Loading : Page
    {
        public Loading()
        {
            InitializeComponent();
            Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(1));

                Application.Current.Dispatcher.Invoke(() =>
                {
                    MainPage mainPage = new MainPage();
                    NavigationService.Navigate(mainPage);
                });
            });
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for Profile.xaml
    /// </summary>
    public partial class Profile : Page, INotifyPropertyChanged
    {
        private string myText;

        public string MyText
        {
            get { return myText; }
            set
            {
                if (myText != value)
                {
                    myText = value;
                    OnPropertyChanged(nameof(MyText));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Profile()
        {
            InitializeComponent();
            DataContext = this; // Set the DataContext to the current instance of the window or ViewModel
            MyText = "John Doe";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Зробити кнопку "Редагувати профіль" невидимою
            editProfileButton.Visibility = Visibility.Collapsed;

            // Зробити видимими кнопки "Скасувати" і "Зберегти"
            cancelButton.Visibility = Visibility.Visible;
            saveButton.Visibility = Visibility.Visible;

            // Робіть TextBox редагованим
            userNameTextBox.IsReadOnly = false;

            // Змініть фокус на TextBox і виділіть весь текст
            userNameTextBox.Focus();
            userNameTextBox.SelectAll();
        }
        private void UserNameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            // Після втрати фокусу TextBox стає нередагованим
            userNameTextBox.IsReadOnly = true;
        }

        private void UserNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            // Якщо користувач натискає клавішу Enter, TextBox також стає нередагованим
            if (e.Key == Key.Enter)
            {
                userNameTextBox.IsReadOnly = true;
                // Ви можете також обробити новий текст, якщо потрібно
            }
        }

        //private void UpdateTextButton_Click(object sender, RoutedEventArgs e)
        //{
        //    MyText = textBox.Text; // Update text from the TextBox
        //}


        //private void UserNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        //{

        //}

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new UserInterface.MainPage());
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new UserInterface.Settings());
        }

        private void UserNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void userNameTextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }
    }
}

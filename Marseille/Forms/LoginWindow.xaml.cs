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
using System.Windows.Shapes;
using HotelDB;

namespace Marseille.Forms
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            loginButton.BorderBrush = new SolidColorBrush(Colors.Transparent);
        }

        private void loginButton_Initialized(object sender, EventArgs e)
        {
            loginButton.BorderBrush = new SolidColorBrush(Colors.Transparent);
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            if (DBConnection.Login(loginBox.Text, passwordBox.Password))
            {
                MessageBox.Show("ALL GOOD!!!!!");
            }
        }
    }
}
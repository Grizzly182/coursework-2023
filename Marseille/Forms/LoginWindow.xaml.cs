using System;
using System.Windows;
using System.Windows.Media;
using HotelDB;
using System.Windows.Input;
using System.Text;
using System.Windows.Controls;
using System.Drawing;

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
            loginBox.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, OnPaste));
            passwordBox.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, OnPaste));
        }

        //TODO: Попробовать ограничить вставку текста не запрещая её полностью.
        private void OnPaste(object sender, ExecutedRoutedEventArgs e)
        {
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
            else
            {
                Viewbox viewbox = new Viewbox();
                Label loginOrPasswordErrorLabel = new Label();
                loginOrPasswordErrorLabel.Content = "Неверный логин или пароль";
                loginOrPasswordErrorLabel.Foreground = new SolidColorBrush(Colors.Red);
                loginOrPasswordErrorLabel.FontFamily = new System.Windows.Media.FontFamily("Segoe UI Semibold");
                loginOrPasswordErrorLabel.FontSize = 14;
                viewbox.Child = loginOrPasswordErrorLabel;

                bottomGrid.Children.Add(viewbox);
                Grid.SetRow(viewbox, 1);
            }
        }

        private void registrationTextBlock_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("all good");
        }

        private void loginBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            foreach (char ch in e.Text.ToLower())
            {
                if (!(ch >= 97 && ch <= 122 || char.IsDigit(ch)))
                {
                    e.Handled = true;
                    return;
                }
            }
        }

        private void passwordBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            foreach (char ch in e.Text.ToLower())
            {
                if (!(ch >= 97 && ch <= 122 || char.IsDigit(ch)))
                {
                    e.Handled = true;
                    return;
                }
            }
        }
    }
}
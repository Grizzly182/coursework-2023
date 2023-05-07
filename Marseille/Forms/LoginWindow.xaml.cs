using System;
using System.Windows;
using System.Windows.Media;
using HotelDB;
using System.Windows.Input;
using Marseille.Assets;

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
            //loginButton.BorderBrush = new SolidColorBrush(Colors.Transparent);
            loginBox.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, OnPaste));
            passwordBox.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, OnPaste));
        }

        private void OnPaste(object sender, ExecutedRoutedEventArgs e)
        {
        }

        private void loginButton_Initialized(object sender, EventArgs e)
        {
            loginButton.BorderBrush = new SolidColorBrush(Colors.Transparent);
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = loginBox.Text;
            string password = passwordBox.Password;
            if (DBConnection.Login(login, password))
            {
                try
                {
                    DBConnection.GetUserData(login, out string name, out string surname, out string patronymic, out int role);
                    User.SetLoggedUser(login, name, surname, patronymic, (Role)role);
                    WindowsController.OpenWindow(this, new AdminWindow());
                    MessageBox.Show(User.Login + User.Name + User.Patronymic + User.Surname + User.UserRole.ToString());
                }
                catch (Exception ex)
                {
                    ErrorMessagesProider.ShowError("Произошла ошибка при входе\n" + ex.Message);
                }
            }
            else
            {
                loginBox.Clear();
                passwordBox.Clear();
                ErrorMessagesProider.ShowError("Неверные логин или пароль.");
            }
        }

        private void registrationTextBlock_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("all good");
        }

        private void loginBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!IsEnglishText(e.Text))
            {
                e.Handled = true;
                return;
            }
        }

        private void passwordBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!IsEnglishText(e.Text))
            {
                e.Handled = true;
                return;
            }
        }

        /// <summary>
        /// Проверяет всю строку на наличие букв, отличающихся от английского алфавита.
        /// </summary>
        /// <param name="text">Строка передаваемая на проверку.</param>
        /// <returns>Возвращает <see langword="true"/> если в строке все буквы
        /// являются частью английского алфавита, иначе возвращает <see langword="false"/></returns>
        private bool IsEnglishText(string text)
        {
            foreach (char ch in text.ToLower())
            {
                if (!(ch >= 97 && ch <= 122 || char.IsDigit(ch)))
                {
                    return false;
                }
            }
            return true;
        }

        private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
        }
    }
}
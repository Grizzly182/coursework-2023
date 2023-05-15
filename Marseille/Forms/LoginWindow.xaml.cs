using System;
using System.Windows;
using System.Windows.Media;
using HotelDB;
using System.Windows.Input;
using Marseille.Assets;
using System.Collections.Generic;

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
            if (IsolatedStorageController.Read("login") != null)
            {
                try
                {
                    string login = IsolatedStorageController.Read("login");
                    string password = IsolatedStorageController.Read("password");

                    if (DBConnection.Login(login, password))
                    {
                        DBConnection.GetUserData(login, out uint id, out string name, out string surname, out string patronymic, out int role);
                        User.SetLoggedUser(id, login, name, surname, patronymic, (Role)role);

                        switch (User.CurrentUser.UserRole)
                        {
                            case Role.Admin:
                                AdminWindow adminWindow = new AdminWindow();
                                Hide();
                                adminWindow.Show();

                                return;

                            case Role.Manager:
                                ManagerWindow managerWindow = new ManagerWindow();
                                Hide();
                                managerWindow.Show();

                                return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorMessagesProider.ShowError("Ошибка при входе.\n" + ex.Message);
                }
            }

            InitializeComponent();
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

            try
            {
                if (DBConnection.Login(login, password))
                {
                    if ((bool)rememberMeCheckBox.IsChecked)
                    {
                        IsolatedStorageController.IsolatedStorageEnabled = true;

                        IsolatedStorageController.Save(new Dictionary<string, string>(){
                            { "login", login},
                            { "password", password }
                        });
                    }

                    DBConnection.GetUserData(login, out uint id, out string name, out string surname, out string patronymic, out int role);
                    User.SetLoggedUser(id, login, name, surname, patronymic, (Role)role);

                    switch (User.CurrentUser.UserRole)
                    {
                        case Role.Admin:
                            AdminWindow adminWindow = new AdminWindow();
                            Hide();
                            adminWindow.Show();
                            break;

                        case Role.Manager:
                            ManagerWindow managerWindow = new ManagerWindow();
                            Hide();
                            managerWindow.Show();
                            break;

                        default: break;
                    }
                }
                else
                {
                    loginBox.Clear();
                    passwordBox.Clear();
                    ErrorMessagesProider.ShowError("Неверные логин или пароль.");
                }
            }
            catch (Exception ex)
            {
                ErrorMessagesProider.ShowError("Произошла ошибка при входе\n" + ex.Message);
            }
        }

        private void registrationTextBlock_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("all good");
        }

        private void loginBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Assets.Utility.IsEnglishText(e.Text))
            {
                e.Handled = true;
                return;
            }
        }

        private void passwordBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Assets.Utility.IsEnglishText(e.Text))
            {
                e.Handled = true;
                return;
            }
        }

        private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
        }
    }
}
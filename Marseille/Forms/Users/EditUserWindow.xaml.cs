using HotelDB;
using Marseille.Assets;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Marseille.Forms
{
    /// <summary>
    /// Логика взаимодействия для EditUserWindow.xaml
    /// </summary>
    public partial class EditUserWindow : Window
    {
        private User user;

        public bool IsCurrentUser { get => user.UserRole != User.CurrentUser.UserRole; }

        public EditUserWindow(User user)
        {
            InitializeComponent();
            this.user = user;
            loginTextBox.Text = user.Login;
            nameTextBox.Text = user.Name;
            surnameTextBox.Text = user.Surname;
            patronymicTextBox.Text = user.Patronymic;

            switch (user.UserRole)
            {
                case Role.Admin:
                    roleComboBox.SelectedIndex = 0;
                    break;

                case Role.Manager:
                    roleComboBox.SelectedIndex = 1;
                    break;
            }

            roleComboBox.IsEnabled = IsCurrentUser;
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            string login = loginTextBox.Text;
            string password = passwordBox.Password;
            string name = nameTextBox.Text;
            string surname = surnameTextBox.Text;
            string patronymic = patronymicTextBox.Text;
            Role role = Role.Admin;

            switch ((string)((ComboBoxItem)roleComboBox.SelectedValue).Content)
            {
                case "Администратор":
                    role = Role.Admin;
                    break;

                case "Управляющий":
                    role = Role.Manager;
                    break;
            }

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(surname))
            {
                ErrorMessagesProider.ShowError("Введены не все данные.");
                return;
            }

            try
            {
                if (string.IsNullOrEmpty(password))
                {
                    DBConnection.EditUser(user.Id, login, name, surname, (int)role, patronymic);
                }
                else
                {
                    DBConnection.EditUser(user.Id, login, name, surname, (int)role, patronymic, password);
                }

                this.DialogResult = true;
            }
            catch (System.Exception ex)
            {
                ErrorMessagesProider.ShowError("Непредвиденная ошибка!\n" + ex.Message);
            }
        }

        private void loginTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
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

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
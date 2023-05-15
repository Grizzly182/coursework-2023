using HotelDB;
using Marseille.Assets;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Marseille.Forms
{
    /// <summary>
    /// Логика взаимодействия для CreateUserWindow.xaml
    /// </summary>
    public partial class CreateUserWindow : Window
    {
        public CreateUserWindow()
        {
            InitializeComponent();
        }

        private void createButton_Click(object sender, RoutedEventArgs e)
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

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(surname))
            {
                ErrorMessagesProider.ShowError("Введены не все данные.");
                return;
            }

            if (DBConnection.ContainsUser(login))
            {
                ErrorMessagesProider.ShowError($"Пользователь {login} уже существует!");
                loginTextBox.Clear();
                return;
            }

            try
            {
                DBConnection.CreateUser(login, password, name, surname, (int)role, patronymic);
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
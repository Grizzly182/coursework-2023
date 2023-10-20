using HotelDB;
using Marseille.Assets;
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

namespace Marseille.Forms
{
    /// <summary>
    /// Логика взаимодействия для CreateClientWindow.xaml
    /// </summary>
    public partial class CreateClientWindow : Window
    {
        private ReservationCreateWindow _parent;

        public CreateClientWindow(ReservationCreateWindow parent)
        {
            _parent = parent;
            InitializeComponent();
        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            string name = nameTextBox.Text;
            string surname = surnameTextBox.Text;
            string patronymic = patronymicTextBox.Text;
            string phone = phoneTextBox.Text;
            DateTime birthday;

            if (!DateTime.TryParse(birthdayDatePicker.Text, out birthday))
            {
                ErrorMessagesProider.ShowError("Неверный формат даты.");
                return;
            }

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(surname) || string.IsNullOrEmpty(birthdayDatePicker.Text))
            {
                ErrorMessagesProider.ShowError("Введены не все данные!");
                return;
            }

            if (GetDifferenceInYears(birthday, DateTime.Today) < 18)
            {
                ErrorMessagesProider.ShowError("Лицу нет 18-ти лет.");
                return;
            }

            try
            {
                Client client = new Client(birthday, name, surname, patronymic, phone);
                _parent.Clients.Add(client);
                DialogResult = true;
            }
            catch (Exception ex)
            {
                ErrorMessagesProider.ShowError("Произошла ошибка!\n" + ex.Message);
                return;
            }
        }

        private int GetDifferenceInYears(DateTime startDate, DateTime endDate)
        {
            return (endDate.Year - startDate.Year - 1) +
                (((endDate.Month > startDate.Month) ||
                ((endDate.Month == startDate.Month) && (endDate.Day >= startDate.Day))) ? 1 : 0);
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
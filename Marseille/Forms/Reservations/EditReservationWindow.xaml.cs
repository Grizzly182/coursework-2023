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
    /// Логика взаимодействия для EditReservationWindow.xaml
    /// </summary>
    public partial class EditReservationWindow : Window
    {
        private Reservation _reservation;

        public EditReservationWindow(Reservation reservation)
        {
            _reservation = reservation;
            InitializeComponent();
            switch (_reservation.Status)
            {
                case ReservationStatus.PaymentWaiting:
                    statusComboBox.SelectedIndex = 0;
                    break;

                case ReservationStatus.Paid:
                    statusComboBox.SelectedIndex = 1;
                    break;

                case ReservationStatus.Canceled:
                    statusComboBox.SelectedIndex = 2;
                    break;

                case ReservationStatus.Completed:
                    statusComboBox.SelectedIndex = 3;
                    break;
            }
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = statusComboBox.SelectedIndex;

            switch (selectedIndex)
            {
                case 0: // Ожидает оплаты
                    DBConnection.SetReservationStatus(_reservation.Id, (uint)ReservationStatus.PaymentWaiting);
                    DBConnection.SetRoomStatus(_reservation.Room.Id, (uint)RoomStatus.Reservated);
                    break;

                case 1: // Оплачено
                    DBConnection.SetReservationStatus(_reservation.Id, (uint)ReservationStatus.Paid);
                    DBConnection.SetRoomStatus(_reservation.Room.Id, (uint)RoomStatus.Reservated);
                    break;

                case 2: //Отменено
                    DBConnection.SetReservationStatus(_reservation.Id, (uint)ReservationStatus.Canceled);
                    DBConnection.SetRoomStatus(_reservation.Room.Id, (uint)RoomStatus.Available);
                    break;

                case 3: //Завершено
                    DBConnection.SetReservationStatus(_reservation.Id, (uint)ReservationStatus.Completed);
                    DBConnection.SetRoomStatus(_reservation.Room.Id, (uint)RoomStatus.Available);
                    break;
            }
            DialogResult = true;
        }
    }
}
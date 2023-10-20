using HotelDB;
using Marseille.Assets;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для ReservationCreateWindow.xaml
    /// </summary>
    public partial class ReservationCreateWindow : Window
    {
        public List<Client> Clients { get; set; }
        private List<Room> _rooms;

        public ReservationCreateWindow()
        {
            _rooms = GetAvailableRoomsFromDatabase();
            Clients = new List<Client>();
            InitializeComponent();
            if (_rooms.Count == 0)
            {
                ErrorMessagesProider.ShowError("Нет свободных комнат.");
                DialogResult = false;
            }

            clientsListView.ItemsSource = Clients;
            roomsComboBox.ItemsSource = _rooms;
            roomsComboBox.SelectedIndex = 0;

            checkInDatePicker.BlackoutDates.Add(new CalendarDateRange(new DateTime(1, 1, 1), DateTime.Today));
            checkOutDatePicker.BlackoutDates.Add(new CalendarDateRange(new DateTime(1, 1, 1), DateTime.Today.AddDays(1)));
        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime time = DateTime.Today;
            DateTime checkInDate, checkOutDate;
            uint userId = User.CurrentUser.Id;
            ReservationStatus status = ReservationStatus.PaymentWaiting;
            Room room = roomsComboBox.SelectedItem as Room;

            switch (statusComboBox.SelectedIndex)
            {
                case 0: // Ожидание оплаты
                    status = ReservationStatus.PaymentWaiting;
                    break;

                case 1: // Оплачена
                    status = ReservationStatus.Paid;
                    break;
            }

            if (!DateTime.TryParse(checkInDatePicker.Text, out checkInDate) | !DateTime.TryParse(checkOutDatePicker.Text, out checkOutDate))
            {
                ErrorMessagesProider.ShowError("Неверный формат даты.");
                return;
            }

            if (Clients.Count == 0)
            {
                ErrorMessagesProider.ShowError("Должен быть как минимум, один жилец");
                return;
            }

            try
            {
                DBConnection.CreateReservation(time.Date, checkInDate.Date, checkOutDate.Date, room.Id, userId, (uint)status);
                var reservations = DBConnection.GetAllReservationsIds();
                uint reservationId = reservations[reservations.Count - 1];

                foreach (Client client in Clients)
                {
                    DBConnection.CreateClient(client.Name, client.Surname, client.Patronymic, client.Birthday.Date, client.Phone);
                    var clients = DBConnection.GetAllClientsIds();
                    uint clientId = clients[clients.Count - 1];

                    DBConnection.AddClientToReservation(clientId, reservationId);
                }
                DBConnection.SetRoomStatus(room.Id, (uint)RoomStatus.Reservated);
                DialogResult = true;
            }
            catch (Exception ex)
            {
                ErrorMessagesProider.ShowError("Произошла ошибка!\n" + ex.Message);
            }
        }

        private List<Room> GetAvailableRoomsFromDatabase()
        {
            List<uint> roomIds = DBConnection.GetRoomIds();
            List<Room> rooms = new List<Room>();

            foreach (uint id in roomIds)
            {
                Dictionary<uint, string> featuresFromDB = DBConnection.GetRoomFeatures(id);
                List<RoomFeature> features = RoomFeature.FromDictionary(featuresFromDB);

                DBConnection.GetRoomData(id, out uint number, out decimal cost, out int beds, out string type, out string description, out int status);
                Room room = new Room(id, number, cost, beds, type, description, (RoomStatus)status, features);

                if (room.Status == RoomStatus.Available)
                {
                    rooms.Add(room);
                }
            }
            return rooms;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void addClientButton_Click(object sender, RoutedEventArgs e)
        {
            CreateClientWindow createClientWindow = new CreateClientWindow(this);
            if (createClientWindow.ShowDialog() == true)
            {
                clientsListView.Items.Refresh();
            }
        }

        private void removeClientButton_Click(object sender, RoutedEventArgs e)
        {
            Client selectedItem = (Client)clientsListView.SelectedItem;
            if (selectedItem != null)
            {
                if (ErrorMessagesProider.ShowWarning("Вы уверены?") == MessageBoxResult.OK)
                {
                    Clients.Remove(selectedItem);
                    clientsListView.Items.Refresh();
                }
            }
        }
    }
}
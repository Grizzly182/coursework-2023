using HotelDB;
using Marseille.Assets;
using Marseille.Custom_Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Windows;
using System.Windows.Controls;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using LiveChartsCore.Defaults;
using System.Security.Cryptography;
using LiveChartsCore.Kernel.Sketches;
using System.Collections.ObjectModel;

namespace Marseille.Forms
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private bool _logoutClicked = false;
        private List<ReservationControl> _reservations;

        public IEnumerable<ISeries> Series = new ISeries[]
        {
            new ColumnSeries<DateTimePoint>
            {
                TooltipLabelFormatter =
                    chartPoint => $"{new DateTime((long) chartPoint.SecondaryValue):MMMM dd}: {chartPoint.PrimaryValue}",
                Values = new ObservableCollection<DateTimePoint>
                {
                    new DateTimePoint(new DateTime(2021, 1, 1), 3),
                    new DateTimePoint (new DateTime(2021, 1, 3), 6),
                    new DateTimePoint(new DateTime(2021, 1, 4), 5),
                    new DateTimePoint(new DateTime(2021, 1, 5), 3),
                    new DateTimePoint(new DateTime(2021, 1, 6), 5),
                    new DateTimePoint(new DateTime(2021, 1, 7), 8),
                    new DateTimePoint(new DateTime(2021, 1, 8), 6)
                },
            }
        };

        public IEnumerable<ICartesianAxis> XAxes = new Axis[]
{
            new Axis
            {
                Labeler = value => new DateTime((long) value).ToString("MMMM dd"),
                LabelsRotation = 15,
                UnitWidth = TimeSpan.FromDays(1).Ticks,
                MinStep = TimeSpan.FromDays(1).Ticks
            }
};

        public void LoadStatistics(DateTime startDate, DateTime endDate)
        {
            List<Reservation> reservations = GetReservationsFromDatabase();

            ObservableCollection<DateTimePoint> points = new ObservableCollection<DateTimePoint>();
            foreach (var reservation in reservations)
            {
                if (reservation.CreateTime > startDate && reservation.CreateTime < endDate)
                {
                    points.Add(new DateTimePoint(reservation.CreateTime, 1));
                }
            }
            IEnumerable<ISeries> series = new ISeries[]
            {
                new ColumnSeries<DateTimePoint>
            {
                TooltipLabelFormatter =
                    chartPoint => $"{new DateTime((long) chartPoint.SecondaryValue):MMMM dd}: {chartPoint.PrimaryValue}",
                Values = points
            }
            };

            lineGraph.Series = series;
        }

        public AdminWindow()
        {
            InitializeComponent();
            lineGraph.Series = Series;
            lineGraph.XAxes = XAxes;
            fullNameTextBlock.Text = User.CurrentUser.ShortFullName;
            _reservations = new List<ReservationControl>();

            reservationsListView.ItemsSource = _reservations;

            scheduleStatusComboBox.SelectionChanged += (sender, e) =>
            {
                if (e.OriginalSource is ComboBox)
                {
                    SortReservations();
                }
            };
            LoadReservations();
            LoadStatistics(startDate.SelectedDate.Value, endDate.SelectedDate.Value);
        }

        private void SortReservations()
        {
            List<ReservationControl> itemsSource = (List<ReservationControl>)reservationsListView.ItemsSource;
            switch (scheduleStatusComboBox.SelectedIndex)
            {
                //case 0: // Ожидание оплаты
                //    itemsSource.Sort((a, b) =>
                //    {
                //        if (a.Reservation.Status == ReservationStatus.PaymentWaiting)
                //        {
                //            return -1;
                //        }
                //        else if (b.Reservation.Status == ReservationStatus.Paid)
                //        {
                //            return 1;
                //        }
                //        return 0;
                //    });
                //    break;

                //case 1: // Отменено
                //    itemsSource.Sort((a, b) =>
                //    {
                //        if (a.Reservation.Status == ReservationStatus.Canceled)
                //        {
                //            return -1;
                //        }
                //        else if (b.Reservation.Status == ReservationStatus.Completed)
                //        {
                //            return 1;
                //        }
                //        return 0;
                //    });

                //    break;

                //case 2: // Оплачено
                //    itemsSource.Sort((a, b) =>
                //    {
                //        if (a.Reservation.Status == ReservationStatus.Paid)
                //        {
                //            return -1;
                //        }
                //        else if (b.Reservation.Status == ReservationStatus.Completed)
                //        {
                //            return 1;
                //        }
                //        return 0;
                //    });
                //    break;

                //case 3: // Завершено
                //    itemsSource.Sort((a, b) =>
                //    {
                //        if (a.Reservation.Status == ReservationStatus.Completed)
                //        {
                //            return -1;
                //        }
                //        else if (b.Reservation.Status == ReservationStatus.PaymentWaiting)
                //        {
                //            return 1;
                //        }
                //        return 0;
                //    });
                //    break;

                case 0:
                    itemsSource.Sort((a, b) => (b.Reservation.CreateTime.CompareTo(a.Reservation.CreateTime)));
                    break;

                case 1: // Старые
                    itemsSource.Sort((a, b) => (a.Reservation.CreateTime.CompareTo(b.Reservation.CreateTime)));
                    break;
            }
            reservationsListView.Items.Refresh();
        }

        private void LoadReservations()
        {
            _reservations.Clear();

            List<Reservation> reservations = GetReservationsFromDatabase();

            foreach (Reservation reservation in reservations)
            {
                ReservationControl reservationControl = new ReservationControl(reservation);

                reservationControl.editButton.Click += (sender, e) =>
                {
                    EditReservationWindow editReservationWindow = new EditReservationWindow(reservation);
                    if (editReservationWindow.ShowDialog() == true)
                    {
                        LoadReservations();
                    }
                };
                _reservations.Add(reservationControl);
            }
            reservationsListView.Items.Refresh();
        }

        private List<Reservation> GetReservationsFromDatabase()
        {
            List<uint> reservationsIds = DBConnection.GetAllReservationsIds();
            List<Reservation> reservations = new List<Reservation>();

            foreach (uint id in reservationsIds)
            {
                List<Client> clients = new List<Client>();
                List<uint> clientsIds = DBConnection.GetReservationClientsIds(id);

                foreach (uint clientId in clientsIds)
                {
                    DBConnection.GetClientData(clientId, out string name, out string surname, out string patronymic, out DateTime birthday, out string phone);
                    Client client = new Client(clientId, birthday, name, surname, patronymic, phone);
                    clients.Add(client);
                }
                DBConnection.GetReservationData(id, out DateTime time, out DateTime checkInDate, out DateTime checkOutDate, out uint roomId, out uint userId, out uint statusId);
                DBConnection.GetUserData(userId, out string login, out string userName, out string userSurname, out string userPatronymic, out int role);
                DBConnection.GetRoomData(roomId, out uint number, out decimal cost, out int beds, out string type, out string description, out int roomStatus);
                User user = new User(userId, login, userName, userSurname, userPatronymic, (Role)role);
                Room room = new Room(roomId, number, cost, beds, type, description, (RoomStatus)roomStatus, new List<RoomFeature>());
                reservations.Add(new Reservation(id, time, checkInDate, checkOutDate, room, (ReservationStatus)statusId, clients, user));
            }
            return reservations;
        }

        private void adminWindow_Closed(object sender, EventArgs e)
        {
            if (_logoutClicked)
            {
                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            }
            App.Current.Shutdown();
        }

        private void adminWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (ErrorMessagesProider.ShowWarning("Вы действительно хотите выйти?") != MessageBoxResult.OK)
            {
                e.Cancel = true;
            }
        }

        private void logoutButton_Click(object sender, RoutedEventArgs e)
        {
            User.LogOut();
            _logoutClicked = true;
            Close();
        }

        private void searchTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            string searchTerm = searchTextBox.Text.ToLower();
            var temp = _reservations.Where(t => (t.Reservation.ClientsToString.ToLower().Contains(searchTerm) || t.Reservation.Id.ToString().ToLower().Contains(searchTerm) ||
            t.Reservation.Room.Number.ToString().ToLower().Contains(searchTerm) || t.Reservation.Status.ToString().ToLower().Contains(searchTerm) ||
            t.Reservation.CreateTime.ToString().ToLower().Contains(searchTerm))).ToList();

            reservationsListView.ItemsSource = temp;
        }

        private void addReservationButton_Click(object sender, RoutedEventArgs e)
        {
            ReservationCreateWindow reservationCreateWindow = new ReservationCreateWindow();

            reservationCreateWindow.Closed += (s, args) => { LoadReservations(); };
            reservationCreateWindow.ShowDialog();
        }

        private void adminTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                switch (adminTabControl.SelectedIndex)
                {
                    case 0: // Users Tab
                        titleTextBlock.Text = "Брони";
                        LoadReservations();
                        break;

                    case 1: // Sttistics Tab
                        titleTextBlock.Text = "Статистика";
                        LoadStatistics(startDate.SelectedDate.Value, endDate.SelectedDate.Value);
                        break;
                }
            }
        }

        private void refreshGraphButton_Click(object sender, RoutedEventArgs e)
        {
            LoadStatistics(startDate.SelectedDate.Value, endDate.SelectedDate.Value);
        }
    }
}
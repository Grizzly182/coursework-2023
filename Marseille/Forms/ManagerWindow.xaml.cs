using HotelDB;
using Marseille.Assets;
using Marseille.Custom_Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Marseille.Forms
{
    /// <summary>
    /// Логика взаимодействия для ManagerWindow.xaml
    /// </summary>
    public partial class ManagerWindow : Window
    {
        private bool _logoutClicked = false;

        private List<UsersControl> _users = new List<UsersControl>();
        private List<RoomControl> _rooms = new List<RoomControl>();

        public ManagerWindow()
        {
            InitializeComponent();
            fullNameTextBlock.Text = User.CurrentUser.ShortFullName;

            usersListView.ItemsSource = _users;
            roomsListView.ItemsSource = _rooms;
            LoadUsers();
            LoadRooms();

            usersFilterComboBox.SelectionChanged += (sender, e) =>
            {
                if (e.OriginalSource is ComboBox)
                {
                    SortUsers();
                }
            };

            roomFilterComboBox.SelectionChanged += (sender, e) =>
            {
                if (e.OriginalSource is ComboBox)
                {
                    SortRooms();
                }
            };
            SortUsers();
        }

        #region Logging Out and Exit

        private void managerWindow_Closed(object sender, EventArgs e)
        {
            if (_logoutClicked)
            {
                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            }
            App.Current.Shutdown();
        }

        private void managerWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
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

        #endregion Logging Out and Exit

        private void managerTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                switch (managerTabControl.SelectedIndex)
                {
                    case 0: // Users Tab
                        titleTextBlock.Text = "Пользователи";
                        LoadUsers();
                        break;

                    case 1: // Rooms Tab
                        titleTextBlock.Text = "Комнаты";
                        LoadRooms();
                        break;

                    case 2: // Sttistics Tab
                        titleTextBlock.Text = "Статистика";
                        break;
                }
            }
        }

        private List<Room> GetRoomsFromDatabase()
        {
            List<uint> roomIds = DBConnection.GetRoomIds();
            List<Room> rooms = new List<Room>();

            foreach (uint id in roomIds)
            {
                Dictionary<uint, string> featuresFromDB = DBConnection.GetRoomFeatures(id);
                List<RoomFeature> features = RoomFeature.FromDictionary(featuresFromDB);

                DBConnection.GetRoomData(id, out uint number, out decimal cost, out int beds, out string type, out string description, out int status);
                Room room = new Room(id, number, cost, beds, type, description, (RoomStatus)status, features);
                rooms.Add(room);
            }
            return rooms;
        }

        private void LoadRooms()
        {
            _rooms.Clear();
            List<Room> rooms = GetRoomsFromDatabase();

            foreach (Room room in rooms)
            {
                RoomControl roomControl = new RoomControl(room);

                roomControl.editButton.Click += (sender, e) =>
                {
                    try
                    {
                        RoomEditWindow roomEditWindow = new RoomEditWindow(room);
                        roomEditWindow.Closed += (s, args) => { LoadRooms(); };
                        roomEditWindow.ShowDialog();
                    }
                    catch (Exception ex)
                    {
                        ErrorMessagesProider.ShowError("Ошибка редактирования:\n" + ex.Message);
                    }
                };

                roomControl.deleteButton.Click += (sender, e) =>
                {
                    try
                    {
                        if (ErrorMessagesProider.ShowWarning("Вы действительно хотите удалить эту комнату?") == MessageBoxResult.OK)
                        {
                            DBConnection.DeleteRoom(roomControl.Room.Id);
                            LoadRooms();
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorMessagesProider.ShowError("Ошибка удаления:\n" + ex.Message);
                    }
                };
                _rooms.Add(roomControl);
            }
            SortRooms();
        }

        private List<User> GetUsersFromDatabase()
        {
            List<string> logins = DBConnection.GetAllUsersLogin();
            List<User> users = new List<User>();

            foreach (string login in logins)
            {
                DBConnection.GetUserData(login, out uint id, out string name, out string surname, out string patronymic, out int role);

                User user = new User(id, login, name, surname, patronymic, (Role)role);
                users.Add(user);
            }

            return users;
        }

        private void LoadUsers()
        {
            _users.Clear();
            List<User> users = GetUsersFromDatabase();
            usersListView.ItemsSource = _users;

            foreach (User user in users)
            {
                UsersControl usersControl = new UsersControl(user);

                usersControl.deleteButton.Click += (sender, e) =>
                {
                    try
                    {
                        if (ErrorMessagesProider.ShowWarning("Вы действительно хотите удалить этого пользователя?") == MessageBoxResult.OK)
                        {
                            DBConnection.DeleteUser(usersControl.User.Id);
                            LoadUsers();
                            SortUsers();
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorMessagesProider.ShowError("Ошибка удаления:\n" + ex.Message);
                    }
                };

                usersControl.editButton.Click += (sender, e) =>
                {
                    try
                    {
                        EditUserWindow editUserWindow = new EditUserWindow(user);
                        editUserWindow.Closed += (s, args) => { LoadUsers(); };
                        editUserWindow.ShowDialog();
                    }
                    catch (Exception ex)
                    {
                        ErrorMessagesProider.ShowError("Ошибка редактирования:\n" + ex.Message);
                    }
                };
                _users.Add(usersControl);
            }
            SortUsers();
        }

        private void addUserButton_Click(object sender, RoutedEventArgs e)
        {
            CreateUserWindow createUserWindow = new CreateUserWindow();

            createUserWindow.Closed += (s, args) => { LoadUsers(); };
            createUserWindow.ShowDialog();
        }

        private void SortUsers()
        {
            List<UsersControl> itemsSource = (List<UsersControl>)usersListView.ItemsSource;
            switch (usersFilterComboBox.SelectedIndex)
            {
                case 0: // ID
                    itemsSource.Sort((a, b) =>
                    {
                        if (a.User.Id < b.User.Id)
                        {
                            return -1;
                        }
                        else if (a.User.Id > b.User.Id)
                        {
                            return 1;
                        }
                        return 0;
                    });
                    break;

                case 1: // Роль
                    itemsSource.Sort((a, b) =>
                    {
                        if (a.User.UserRole < b.User.UserRole)
                        {
                            return -1;
                        }
                        else if (a.User.UserRole > b.User.UserRole)
                        {
                            return 1;
                        }
                        return 0;
                    });

                    break;
            }
            usersListView.Items.Refresh();
        }

        private void SortRooms()
        {
            List<RoomControl> itemsSource = (List<RoomControl>)roomsListView.ItemsSource;
            switch (roomFilterComboBox.SelectedIndex)
            {
                case 0: // ID
                    itemsSource.Sort((a, b) =>
                    {
                        return a.Room.Id.CompareTo(b.Room.Id);
                    });
                    break;

                case 1: // Номер
                    itemsSource.Sort((a, b) =>
                    {
                        return a.Room.Number.CompareTo(b.Room.Number);
                    });
                    break;

                case 2: // Количество спальных мест
                    itemsSource.Sort((a, b) =>
                    {
                        return b.Room.BedsCount.CompareTo(a.Room.BedsCount);
                    });
                    break;

                case 3: // Стоимость
                    itemsSource.Sort((a, b) =>
                    {
                        return b.Room.CostPerNight.CompareTo(a.Room.CostPerNight);
                    });
                    break;
            }
            roomsListView.Items.Refresh();
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchTerm = searchTextBox.Text;
            List<UsersControl> temp = _users.Where(t => (t.User.Login.Contains(searchTerm) || t.User.Name.ToLower().Contains(searchTerm.ToLower()) ||
            t.User.Surname.ToLower().Contains(searchTerm.ToLower()) || t.User.Patronymic.ToLower().Contains(searchTerm.ToLower()) ||
            t.User.RoleToString.ToLower().Contains(searchTerm.ToLower()))).ToList();

            usersListView.ItemsSource = temp;
            SortUsers();
        }

        private void roomSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchTerm = roomSearchTextBox.Text;
            List<RoomControl> temp = _rooms.Where(t => (t.Room.Id.ToString().Contains(searchTerm.ToLower()) || t.Room.Number.ToString().Contains(searchTerm.ToLower()) ||
            t.Room.BedsCount.ToString().Contains(searchTerm.ToLower()) || t.Room.CostPerNight.ToString().Contains(searchTerm.ToLower()) || t.Room.Type.ToLower().Contains(searchTerm.ToLower()) ||
            t.Room.Description.ToLower().Contains(searchTerm.ToLower()) || t.Room.FeaturesToString.ToLower().Contains(searchTerm.ToLower()) || t.Room.Status.ToString().ToLower().Contains(searchTerm.ToLower()))).ToList();

            roomsListView.ItemsSource = temp;
            SortRooms();
        }

        private void addRoomButton_Click(object sender, RoutedEventArgs e)
        {
            CreateRoomWindow createRoomWindow = new CreateRoomWindow();

            createRoomWindow.Closed += (s, args) => { LoadRooms(); };
            createRoomWindow.ShowDialog();
        }
    }
}
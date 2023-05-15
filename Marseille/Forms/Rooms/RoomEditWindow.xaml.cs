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
    /// Логика взаимодействия для RoomEditWindow.xaml
    /// </summary>
    public partial class RoomEditWindow : Window
    {
        private Room _room;

        private List<RoomFeature> features;
        private List<RoomFeature> featuresList;

        public RoomEditWindow(Room room)
        {
            InitializeComponent();

            _room = room;

            Dictionary<uint, string> featuresFromDB = DBConnection.GetAllRoomFeatures();
            features = RoomFeature.FromDictionary(featuresFromDB);
            featuresList = _room.Features;

            featuresComboBox.ItemsSource = features;
            featuresComboBox.SelectedIndex = 0;

            featuresListView.ItemsSource = featuresList;

            typeTextBox.Text = _room.Type;
            descriptionTextBox.Text = _room.Description;
            numberTextBox.Value = (int)_room.Number;
            bedsCountTextBox.Value = _room.BedsCount;
            costTextBox.Value = (int)_room.CostPerNight;

            numberTextBox.IsEnabled = false;
            if (_room.Status == RoomStatus.Reservated)
                statusComboBox.IsEnabled = false;
        }

        private void RefreshFeatures()
        {
            Dictionary<uint, string> featuresFromDB = DBConnection.GetAllRoomFeatures();
            features = RoomFeature.FromDictionary(featuresFromDB);
            featuresComboBox.ItemsSource = features;
            featuresComboBox.Items.Refresh();
            featuresComboBox.SelectedIndex = 0;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            string type = typeTextBox.Text;
            string description = descriptionTextBox.Text;
            uint? number = (uint?)numberTextBox.Value ?? null;
            uint? bedsCount = (uint?)bedsCountTextBox.Value ?? null;
            decimal? cost = (decimal?)costTextBox.Value ?? null;

            RoomStatus status = _room.Status == RoomStatus.Reservated ? RoomStatus.Reservated : RoomStatus.Available;

            if (_room.Status != RoomStatus.Reservated)
            {
                switch ((string)((ComboBoxItem)statusComboBox.SelectedValue).Content)
                {
                    case "Доступна":
                        status = RoomStatus.Available;
                        break;

                    case "Недоступна":
                        status = RoomStatus.Unavailable;
                        break;

                    default:
                        ErrorMessagesProider.ShowError("Бронировать комнаты могут только администраторы системы.");
                        statusComboBox.SelectedIndex = 0;
                        return;
                }
            }

            if (string.IsNullOrEmpty(type) || number == null || bedsCount == null || cost == null)
            {
                ErrorMessagesProider.ShowError("Введены не все данные.");
                return;
            }

            try
            {
                var features = featuresList;
                DBConnection.EditRoom(_room.Id, (uint)number, (decimal)cost, (int)bedsCount, type, description, (int)status, RoomFeature.ToDictionary(featuresList));
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                ErrorMessagesProider.ShowError("Возникла ошибка!\n" + ex.Message);
            }
        }

        private void addFeatureButton_Click(object sender, RoutedEventArgs e)
        {
            RoomFeature feature = (RoomFeature)featuresComboBox.SelectedItem;
            if (!featuresListView.Items.Contains(feature))
            {
                featuresList.Add(feature);
                featuresListView.Items.Refresh();
            }
        }

        private void removeFeatureButton_Click(object sender, RoutedEventArgs e)
        {
            RoomFeature feature = (RoomFeature)featuresListView.SelectedItem;
            if (featuresListView.Items.Contains(feature))
            {
                featuresList.Remove(feature);
                featuresListView.Items.Refresh();
            }
        }

        private void featurenManageButton_Click(object senderm, RoutedEventArgs e)
        {
            FeaturesManagementWindow featuresManagementWindow = new FeaturesManagementWindow();
            featuresManagementWindow.Closed += (s, args) => { RefreshFeatures(); };
            featuresManagementWindow.ShowDialog();
        }
    }
}
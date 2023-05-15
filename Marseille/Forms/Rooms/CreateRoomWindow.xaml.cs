using HotelDB;
using Marseille.Assets;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

namespace Marseille.Forms
{
    /// <summary>
    /// Логика взаимодействия для CreateRoomWindow.xaml
    /// </summary>
    public partial class CreateRoomWindow : Window
    {
        private List<RoomFeature> features;
        private List<RoomFeature> featuresList;

        public CreateRoomWindow()
        {
            InitializeComponent();
            Dictionary<uint, string> featuresFromDB = DBConnection.GetAllRoomFeatures();
            features = RoomFeature.FromDictionary(featuresFromDB);
            featuresList = new List<RoomFeature>();

            featuresComboBox.ItemsSource = features;
            featuresComboBox.SelectedIndex = 0;

            featuresListView.ItemsSource = featuresList;
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

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            string type = typeTextBox.Text;
            string description = descriptionTextBox.Text;
            uint? number = (uint?)numberTextBox.Value ?? null;
            uint? bedsCount = (uint?)bedsCountTextBox.Value ?? null;
            decimal? cost = (decimal?)costTextBox.Value ?? null;

            RoomStatus status = RoomStatus.Available;

            switch ((string)((ComboBoxItem)statusComboBox.SelectedValue).Content)
            {
                case "Доступна":
                    status = RoomStatus.Available;
                    break;

                case "Недоступна":
                    status = RoomStatus.Unavailable;
                    break;
            }

            if (string.IsNullOrEmpty(type) || number == null || bedsCount == null || cost == null)
            {
                ErrorMessagesProider.ShowError("Введены не все данные.");
                return;
            }

            if (DBConnection.ContainsRoom((uint)number))
            {
                ErrorMessagesProider.ShowError("Комната уже существует");
                numberTextBox.Value = 0;
                return;
            }

            try
            {
                var features = featuresList;
                DBConnection.CreateRoom((uint)number, (decimal)cost, (int)bedsCount, type, description, (int)status, RoomFeature.ToDictionary(features));
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
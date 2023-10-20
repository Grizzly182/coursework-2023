using HotelDB;
using Marseille.Assets;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Marseille.Forms
{
    /// <summary>
    /// Логика взаимодействия для FeaturesManagementWindow.xaml
    /// </summary>
    public partial class FeaturesManagementWindow : Window
    {
        private List<RoomFeature> roomFeatures;

        public FeaturesManagementWindow()
        {
            InitializeComponent();
            roomFeatures = new List<RoomFeature>();
            RefreshList();
            featuresListView.ItemsSource = roomFeatures;
        }

        private void addFeatureButton_Click(object sender, RoutedEventArgs e)
        {
            string featureName = addFeatureTextBox.Text;
            if (DBConnection.ContainsFeature(featureName))
            {
                ErrorMessagesProider.ShowError("Опция уже существует!");
                return;
            }
            if (!string.IsNullOrEmpty(featureName))
            {
                DBConnection.CreateFeature(featureName);
            }
            addFeatureTextBox.Clear();
            RefreshList();
        }

        private void deleteFeatureButton_Click(object sender, RoutedEventArgs e)
        {
            RoomFeature selectedItem = (RoomFeature)featuresListView.SelectedItem;
            if (selectedItem != null)
            {
                if (ErrorMessagesProider.ShowWarning("Вы действительно хотите удалить эту опцию?") == MessageBoxResult.OK)
                {
                    try
                    {
                        DBConnection.DeleteFeature(selectedItem.Id);
                    }
                    catch (Exception ex)
                    {
                        ErrorMessagesProider.ShowError("Ошибка!\n" + ex.Message);
                    }
                }
            }
            RefreshList();
        }

        private void RefreshList()
        {
            roomFeatures.Clear();
            roomFeatures = RoomFeature.FromDictionary(DBConnection.GetAllRoomFeatures());
            featuresListView.ItemsSource = roomFeatures;
            featuresListView.Items.Refresh();
        }

        private void finishButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
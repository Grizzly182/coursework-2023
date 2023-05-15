using Marseille.Assets;
using System;
using System.Windows;

namespace Marseille.Forms
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private bool _logoutClicked = false;

        public AdminWindow()
        {
            InitializeComponent();
            fullNameTextBlock.Text = User.CurrentUser.ShortFullName;
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
    }
}
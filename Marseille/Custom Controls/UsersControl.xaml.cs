using System.Windows.Controls;

namespace Marseille.Custom_Controls
{
    /// <summary>
    /// Логика взаимодействия для UsersControl.xaml
    /// </summary>
    public partial class UsersControl : UserControl
    {
        private User _user;

        public User User
        {
            get => _user;
            private set => _user = value;
        }

        public bool IsCurrentUser
        { get { return this.User.Id != User.CurrentUser.Id; } }

        public UsersControl()
        {
            InitializeComponent();
        }

        public UsersControl(User user)
        {
            _user = user;
            InitializeComponent();
        }
    }
}
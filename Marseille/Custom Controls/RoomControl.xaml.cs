using System.Windows.Controls;

namespace Marseille.Custom_Controls
{
    /// <summary>
    /// Логика взаимодействия для RoomControl.xaml
    /// </summary>
    public partial class RoomControl : UserControl
    {
        private Room _room;

        public Room Room { get => _room; private set => _room = value; }

        public RoomControl(Room room)
        {
            _room = room;
            InitializeComponent();
        }
    }
}
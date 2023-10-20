using System.Windows.Controls;

namespace Marseille.Custom_Controls
{
    /// <summary>
    /// Логика взаимодействия для ReservationControl.xaml
    /// </summary>
    public partial class ReservationControl : UserControl
    {
        private Reservation _reservation;

        public Reservation Reservation
        { get { return _reservation; } private set { _reservation = value; } }

        public ReservationControl()
        { }

        public ReservationControl(Reservation reservation)
        {
            InitializeComponent();
            _reservation = reservation;
            idTextBlock.Text = reservation.Id.ToString();
            createTimeTextBlock.Text = reservation.CreateTime.ToString();
            checkInTextBlock.Text = reservation.CheckInDate.Date.ToString();
            checkOutTextBlock.Text = reservation.CheckOutDate.Date.ToString();
            roomTextBlock.Text = reservation.Room.Number.ToString();
            statusTextBlock.Text = reservation.Status.ToString();
            clientsTextBlock.Text = reservation.ClientsToString;
        }
    }
}
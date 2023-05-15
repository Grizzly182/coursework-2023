using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marseille
{
    public class Reservation
    {
        public uint Id { get; private set; }
        public DateTime CreateTime { get; private set; }
        public DateTime CheckInDate { get; private set; }
        public DateTime CheckOutDate { get; private set; }
        public Room Room { get; private set; }
        public List<Client> Clients { get; private set; }

        public Reservation(uint id, DateTime createTime, DateTime checkInDate, DateTime checkOutDate, Room room)
        {
            Id = id;
            CreateTime = createTime;
            CheckInDate = checkInDate;
            CheckOutDate = checkOutDate;
            Room = room;
        }
    }
}
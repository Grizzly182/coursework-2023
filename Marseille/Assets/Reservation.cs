using Marseille.Assets;
using System;
using System.Collections.Generic;

namespace Marseille
{
    public class Reservation
    {
        public uint Id { get; private set; }
        public DateTime CreateTime { get; private set; }
        public DateTime CheckInDate { get; private set; }
        public DateTime CheckOutDate { get; private set; }
        public Room Room { get; private set; }
        public User User { get; private set; }
        public List<Client> Clients { get; private set; }
        public ReservationStatus Status { get; private set; }

        public string ClientsToString
        {
            get
            {
                string clients = string.Empty;

                if (Clients != null)
                {
                    foreach (Client client in Clients)
                    {
                        clients += client.ToString();
                    }
                }
                return clients;
            }
        }

        public Reservation(uint id, DateTime createTime, DateTime checkInDate, DateTime checkOutDate, Room room, ReservationStatus status, List<Client> clients, User user)
        {
            Id = id;
            CreateTime = createTime;
            CheckInDate = checkInDate;
            CheckOutDate = checkOutDate;
            Clients = clients;
            Room = room;
            Status = status;
            User = user;
        }
    }
}
using Marseille.Assets;
using System.Collections.Generic;
using System.Drawing;

namespace Marseille
{
    public class Room
    {
        public uint Id { get; private set; }

        public uint Number { get; private set; }
        public decimal CostPerNight { get; private set; }
        public int BedsCount { get; private set; }
        public string Type { get; private set; }
        public string Description { get; private set; }
        public RoomStatus Status { get; private set; }
        public List<RoomFeature> Features { get; private set; }

        public string FeaturesToString
        {
            get
            {
                string features = string.Empty;
                foreach (RoomFeature feature in Features)
                {
                    features += feature.Name + "\n";
                }
                return features;
            }
        }

        public string CostToString
        {
            get => string.Format("{0:C}", CostPerNight);
        }

        public Room(uint id, uint number, decimal costPerNight, int bedsCount, string type, string description, RoomStatus status, List<RoomFeature> features)
        {
            Id = id;
            Number = number;
            CostPerNight = costPerNight;
            BedsCount = bedsCount;
            Type = type;
            Description = description;
            Status = status;
            Features = features;
        }
    }
}
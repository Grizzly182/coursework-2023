using System.Collections.Generic;

namespace Marseille.Assets
{
    public class RoomFeature
    {
        public uint Id { get; private set; }
        public string Name { get; private set; }

        public RoomFeature(uint id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }

        public static List<RoomFeature> FromDictionary(Dictionary<uint, string> featuresDictionary)
        {
            List<RoomFeature> featuresList = new List<RoomFeature>();
            foreach (var feature in featuresDictionary)
            {
                featuresList.Add(new RoomFeature(feature.Key, feature.Value));
            }
            return featuresList;
        }

        public static Dictionary<uint, string> ToDictionary(List<RoomFeature> featuresList)
        {
            Dictionary<uint, string> featuresDictionary = new Dictionary<uint, string>();
            foreach (var feature in featuresList)
            {
                featuresDictionary.Add(feature.Id, feature.Name);
            }
            return featuresDictionary;
        }
    }
}
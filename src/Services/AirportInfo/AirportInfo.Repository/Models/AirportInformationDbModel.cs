using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AirportInfo.Repository.Models
{
    /// <summary>
    /// Db model for Save and load airport information
    /// </summary>
    class AirportInformationDbModel: MongoDbModel
    {
        public AirportInformationDbModel()
        {
            SchemaVersion = 1;
        }

        public string Continent { get; set; }

        public GeoCoordinateDbModel Location { get; set; }

        public string ElevationFt { get; set; }

        public string GpsCode { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string IATACode { get; set; }

        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Ident { get; set; }

        public string IsoCountry { get; set; }

        public string IsoRegion { get; set; }

        public string LocalCode { get; set; }

        public string Municipality { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }
    }
}

using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Routes.Repository.Models
{
    /// <summary>
    /// SchemaVersion model for migration table
    /// </summary>
    class VersionInfoDbModel
    {
        public VersionInfoDbModel()
        {

        }

        public VersionInfoDbModel(int version, DateTime appliedOn, string description = null)
        {
            Version = version;
            AppliedOn = appliedOn;
            Description = description;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        public int Version { get; set; }
        
        public DateTime AppliedOn { get; set; }
        
        public string Description { get; set; }
    }
}

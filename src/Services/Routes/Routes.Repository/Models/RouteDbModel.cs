using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Routes.Repository.Models
{
    class RouteDbModel : MongoDbModel
    {
        [BsonId(IdGenerator = typeof(GuidGenerator))]
        [BsonRepresentation(BsonType.String)]
        //[BsonIgnoreIfDefault]
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public ICollection<RoutePointDbModel> Points { get; set; }
    }
}

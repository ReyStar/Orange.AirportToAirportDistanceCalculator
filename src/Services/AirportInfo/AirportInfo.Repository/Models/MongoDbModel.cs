using System;
using System.Collections.Generic;
using System.Text;

namespace AirportInfo.Repository.Models
{
    abstract class MongoDbModel
    {
        public int SchemaVersion { get; protected set; }

        public int Revision { get; set; }
    }
}

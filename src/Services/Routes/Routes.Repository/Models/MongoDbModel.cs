namespace Routes.Repository.Models
{
    abstract class MongoDbModel
    {
        public int SchemaVersion { get; protected set; }

        public int Revision { get; set; }
    }
}

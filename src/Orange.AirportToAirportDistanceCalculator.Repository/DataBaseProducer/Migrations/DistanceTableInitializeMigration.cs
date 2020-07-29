using FluentMigrator;

namespace Orange.AirportToAirportDistanceCalculator.Repository.DataBaseProducer.Migrations
{
    /// <summary>
    /// Migration to create distance table
    /// </summary>
    [Migration(1, "Create distance table")]
    public class DistanceTableInitializeMigration : ForwardOnlyMigration
    {
        private const string TableName = "distance";

        public override void Up()
        {
            Create
                .Table(TableName)
                .WithColumn("departure_iata_code").AsAnsiString(3).NotNullable()
                .WithColumn("destination_iata_code").AsAnsiString(3).NotNullable()
                .WithColumn("distance").AsDouble().NotNullable()
                .WithColumn("server_time").AsDateTime2().WithDefault(SystemMethods.CurrentUTCDateTime)
                ;

            Create
                .Index("UC_distance")
                .OnTable(TableName)
                .WithOptions()
                .Unique()
                .OnColumn("departure_iata_code")
                .Ascending()
                .OnColumn("destination_iata_code")
                .Ascending();
        }
    }
}

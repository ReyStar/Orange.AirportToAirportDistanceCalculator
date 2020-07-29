using Dapper.Dommel.FluentMapping;
using Orange.AirportToAirportDistanceCalculator.Repository.Models;

namespace Orange.AirportToAirportDistanceCalculator.Repository.DapperConfig.Maps
{
    class GeoDistanceDbModelMap : DommelEntityMap<GeoDistanceDbModel>
    {
        //IsKey - this property used by Dommel for control insert or update functions.
        //This orm can use multi column key. We can use natural Id key (concatenation two or more columns)
        //or create some unique key in column, but this key will end up in the domain.
        public GeoDistanceDbModelMap()
        {
            ToTable("distance");
            Map(x => x.DepartureIATACode).ToColumn("departure_iata_code");
            Map(x => x.DestinationIATACode).ToColumn("destination_iata_code").IsKey();
            Map(x => x.Distance).ToColumn("distance");
        }
    }
}

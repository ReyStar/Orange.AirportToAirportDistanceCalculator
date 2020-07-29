using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Orange.AirportToAirportDistanceCalculator.Application.Interfaces;
using Orange.AirportToAirportDistanceCalculator.Repository.Models;

namespace Orange.AirportToAirportDistanceCalculator.Repository.Repositories
{
    /// <summary>
    /// Repository for storing distances between airports
    /// </summary>
    /// https://blog.maskalik.com/asp-net/sqlite-simple-database-with-dapper/
    /// https://docs.microsoft.com/en-us/dotnet/standard/data/sqlite/dapper-limitations
    internal class DistancesRepository: IDistancesRepository
    {
        private readonly IDataSource _dataSource;

        public DistancesRepository(IDataSource dataSource)
        {
            _dataSource = dataSource;
        }

        public async Task<double?> GetDistanceAsync(string departureIATACode, string destinationIATACode, CancellationToken cancellationToken = default)
        {
            var compositeKey = GetCompositeKey(departureIATACode, destinationIATACode);

            // I don't use compatible methods with linq expression, because of they don't have cancellationToken param
            //await _dataSource.Connection.FirstOrDefaultAsync<GeoDistanceDbModel>(x => x.DepartureIATACode == compositeKey.starIATACode && x.DestinationIATACode == compositeKey.endIATACode);

            var geoDistance =
                await _dataSource.Connection.QueryFirstOrDefaultAsync<GeoDistanceDbModel>(
                    new CommandDefinition(
                        "Select departure_iata_code, destination_iata_code, distance from distance where departure_iata_code = @starIATACode AND destination_iata_code = @endIATACode",
                        parameters: new
                        {
                            starIATACode = compositeKey.starIATACode, 
                            endIATACode = compositeKey.endIATACode
                        },
                        cancellationToken: cancellationToken));

            return geoDistance?.Distance;
        }

        public async Task<bool> AddDistanceAsync(string departureIATACode, string destinationIATACode, double distance, CancellationToken cancellationToken = default)
        {
            var compositeKey = GetCompositeKey(departureIATACode, destinationIATACode);

            return await 
                 _dataSource.Connection.ExecuteAsync(
                    new CommandDefinition(
                        "INSERT OR IGNORE INTO distance(departure_iata_code, destination_iata_code, distance) VALUES(@starIATACode, @endIATACode, @distance)",
                        parameters: new
                        {
                            starIATACode = compositeKey.starIATACode,
                            endIATACode = compositeKey.endIATACode,
                            distance = distance
                        },
                        cancellationToken: cancellationToken)) > 0;
        }


        /// <summary>
        /// Get composite key
        /// Swap codes for save optimization (remove duplicates)
        /// </summary>
        private static (string starIATACode, string endIATACode) GetCompositeKey(string departureIATACode, string destinationIATACode)
        {
            if (string.Compare(departureIATACode, destinationIATACode, StringComparison.OrdinalIgnoreCase) > 0)
            {
                return (destinationIATACode, departureIATACode);
            }

            return (departureIATACode, destinationIATACode);
        }
    }
}

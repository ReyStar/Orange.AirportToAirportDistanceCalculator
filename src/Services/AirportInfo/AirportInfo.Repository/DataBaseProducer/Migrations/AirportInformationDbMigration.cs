using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AirportInfo.Repository.Models;
using AirportInfo.Repository.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace AirportInfo.Repository.DataBaseProducer.Migrations
{
    /// <summary>
    /// Migration airport collection V1
    /// </summary>
    class AirportInformationDbMigration : DbMigration<AirportInformationDbModel>
    {
        private readonly IMapper _mapper;

        public AirportInformationDbMigration(IDataSource dataSource, IMapper mapper, ILogger<AirportInformationDbMigration> logger)
            : base(1, "Initializing the airport collection", dataSource, logger)
        {
            _mapper = mapper;
        }

        protected override async Task OnUpAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation($"{nameof(AirportInformationDbMigration)} has started");

            var collection = DataSource.Database.GetCollection<AirportInformationDbModel>(AirportInfoRepository.CollectionName);

            if (!await collection.AsQueryable().AnyAsync(cancellationToken: cancellationToken))
            {
                // Set location indexes
                var builder = Builders<AirportInformationDbModel>.IndexKeys;
                var geoIndex = builder.Geo2D(prop => prop.Location);
                //Unique = true // Cant create Unique index in field with null values
                var iataIndex = builder.Ascending(x => x.IATACode);
                
                var fullTextSearchIndex = builder.Text(x => x.Name)
                                                 .Text(x=>x.IATACode)
                                                 .Text(x=>x.Municipality);
                
                await collection.Indexes.CreateManyAsync(new[]
                {
                    new CreateIndexModel<AirportInformationDbModel>(geoIndex, new CreateIndexOptions {Name = $"{nameof(geoIndex)}"}),
                    new CreateIndexModel<AirportInformationDbModel>(iataIndex, new CreateIndexOptions {Name = $"{nameof(iataIndex)}"}),
                    new CreateIndexModel<AirportInformationDbModel>(fullTextSearchIndex, new CreateIndexOptions { Name = $"{nameof(fullTextSearchIndex)}"}),

                }, cancellationToken);

                await LoadKnownAirportsAsync(collection, cancellationToken);
            }

            Logger.LogInformation($"{nameof(AirportInformationDbMigration)} completed");
        }

        private async Task LoadKnownAirportsAsync(IMongoCollection<AirportInformationDbModel> collection, CancellationToken cancellationToken)
        {
            var assembly = typeof(AirportInformationDbMigration).Assembly;

            await using var stream = assembly.GetManifestResourceStream("AirportInfo.Repository.DataBaseProducer.Migrations.airport-codes.json");
            using StreamReader reader = new StreamReader(stream);

            var text = await reader.ReadToEndAsync();

            var dbModels = JsonConvert.DeserializeObject<IEnumerable<AirportImportModel>>(text).ToArray();

            var index = 0;
            var batchSize = 1000;
            do
            {
                var sequence = dbModels.Skip(index).Take(batchSize);

                var airportInformationDbModels = _mapper.Map<IEnumerable<AirportInformationDbModel>>(sequence);
                var insertsModels = airportInformationDbModels.Select(x => new InsertOneModel<AirportInformationDbModel>(x));

                await collection.BulkWriteAsync(insertsModels, new BulkWriteOptions(), cancellationToken: cancellationToken);

                index += batchSize;
            } while (dbModels.Length > index);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AirportInfo.Domain.Interfaces;
using AirportInfo.Domain.Models;
using AirportInfo.Repository.Models;
using AutoMapper;
using MongoDB.Driver;

namespace AirportInfo.Repository.Repositories
{
    /// <summary>
    /// Repository for storing airports information
    /// </summary>
    internal class AirportInfoRepository: IAirportInfoRepository
    {
        private readonly IDataSource _dataSource;
        private readonly IMapper _mapper;
        private readonly IMongoCollection<AirportInformationDbModel> _mongoCollection;

        public const string CollectionName = "AirportInformation";

        public AirportInfoRepository(IDataSource dataSource, IMapper mapper)
        {
            _dataSource = dataSource;
            _mapper = mapper;
            _mongoCollection = _dataSource.Database.GetCollection<AirportInformationDbModel>(CollectionName);

        }

        /// <summary>
        /// Get airport information by IATA code
        /// </summary>
        public async Task<AirportInformation> GetAirportInfoAsync(string IATACode, CancellationToken cancellationToken = default)
        {
            var cursor =  _mongoCollection.Find(x => x.IATACode == IATACode);
            var airportInformationDbModel = await cursor.FirstOrDefaultAsync(cancellationToken);

            return _mapper.Map<AirportInformation>(airportInformationDbModel);
        }

        /// <summary>
        /// Get airport information use full text search
        /// </summary>
        public async Task<IEnumerable<AirportInformation>> FindAirportInfoUseFullTextAsync(string fuzzyString, CancellationToken cancellationToken = default)
        {
            var cursor = _mongoCollection.Find(Builders<AirportInformationDbModel>.Filter.Text(fuzzyString));
            var airportInformationDbModel = await cursor.Limit(10).ToListAsync(cancellationToken);

            return _mapper.Map<IEnumerable<AirportInformation>>(airportInformationDbModel);
        }

        /// <summary>
        /// Add or update the airport information
        /// </summary>
        public async Task<bool> AddOrUpdateAirportInfoAsync(string IATACode,
                                                            AirportInformation airportInformation,
                                                            CancellationToken cancellationToken = default)
        {
            var airportInformationDbModel = _mapper.Map<AirportInformationDbModel>(airportInformation);
            airportInformationDbModel.IATACode = IATACode;

            var result = await _mongoCollection.ReplaceOneAsync(x => x.IATACode == IATACode, airportInformationDbModel,
                                                                new ReplaceOptions
                                                                {
                                                                    IsUpsert = true
                                                                }, cancellationToken: cancellationToken);
            return result.IsModifiedCountAvailable;
        }
    }
}

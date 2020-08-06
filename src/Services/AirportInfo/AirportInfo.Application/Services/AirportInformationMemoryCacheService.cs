using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AirportInfo.Domain.Interfaces;
using AirportInfo.Domain.Models;
using Microsoft.Extensions.Caching.Memory;

namespace AirportInfo.Application.Services
{
    /// <summary>
    /// In memory caching airport information for the AirportInfoService
    /// </summary>
    class AirportInformationMemoryCacheService : IAirportInfoService
    {
        private readonly IAirportInfoService _airportInfoService;
        private readonly IMemoryCache _memoryCache;
        private readonly MemoryCacheEntryOptions _memoryCacheGeoDistanceEntryOptions;

        public AirportInformationMemoryCacheService(IAirportInfoService airportInfoService, IMemoryCache memoryCache)
        {
            _airportInfoService = airportInfoService;
            _memoryCache = memoryCache;
            
            _memoryCacheGeoDistanceEntryOptions = new MemoryCacheEntryOptions
                                                  {
                                                      Size = 256
                                                  };
        }
      
        public async Task<AirportInformation> GetAirportInfoAsync(string IATACode, CancellationToken cancellationToken)
        {
            if(_memoryCache.TryGetValue<AirportInformation>(IATACode, out var airportInformation))
            {
                return airportInformation;
            }

            airportInformation = await _airportInfoService.GetAirportInfoAsync(IATACode, cancellationToken);

            _memoryCache.Set(IATACode, airportInformation, _memoryCacheGeoDistanceEntryOptions);

            return airportInformation;
        }

        public Task AddOrUpdateAirportInfoAsync(string IATACode, 
                                                AirportInformation airportInformation,
                                                CancellationToken cancellationToken)
        {
            _memoryCache.Remove(IATACode);

            return _airportInfoService.AddOrUpdateAirportInfoAsync(IATACode, airportInformation, cancellationToken);
        }

        public Task<IEnumerable<AirportInformation>> FindAirportInfoUseFullTextSearchAsync(string query, CancellationToken cancellationToken = default)
        {
            return _airportInfoService.FindAirportInfoUseFullTextSearchAsync(query, cancellationToken);
        }
    }
}

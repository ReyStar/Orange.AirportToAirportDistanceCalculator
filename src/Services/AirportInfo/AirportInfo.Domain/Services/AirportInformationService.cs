using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AirportInfo.Domain.Exceptions;
using AirportInfo.Domain.Interfaces;
using AirportInfo.Domain.Models;

namespace AirportInfo.Domain.Services
{
    /// <summary>
    ///  A service that provides information about airports
    /// </summary>
    class AirportInformationService : IAirportInfoService
    {
        private readonly IAirportInfoRepository _airportInfoRepository;

        public AirportInformationService(IAirportInfoRepository airportInfoRepository)
        {
            _airportInfoRepository = airportInfoRepository;
        }

        /// <summary>
        /// Get airport information using IATA.
        /// </summary>
        /// <param name="IATACode">The IATA airport code.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task<AirportInformation> GetAirportInfoAsync(string IATACode, CancellationToken cancellationToken)
        {
            try
            {
                var airportInfo = await _airportInfoRepository.GetAirportInfoAsync(IATACode, cancellationToken);

                return airportInfo;
            }
            catch (TaskCanceledException ex)
            {
                throw new AirportInformationServiceException(ex);
            }
            catch (Exception ex)
            {
                throw new AirportInformationServiceException(ex);
            }
        }

        /// <summary>
        ///  Find the airport information use full text search.
        /// </summary>
        public async Task<IEnumerable<AirportInformation>> FindAirportInfoUseFullTextSearchAsync(string query, CancellationToken cancellationToken)
        {
            try
            {
                var airportInfoList = await _airportInfoRepository.FindAirportInfoUseFullTextAsync(query, cancellationToken);

                return airportInfoList;
            }
            catch (TaskCanceledException ex)
            {
                throw new AirportInformationServiceException(ex);
            }
            catch (Exception ex)
            {
                throw new AirportInformationServiceException(ex);
            }
        }

        public async Task AddOrUpdateAirportInfoAsync(string IATACode, 
                                                      AirportInformation airportInformation,
                                                      CancellationToken cancellationToken)
        {
            try
            {
                await _airportInfoRepository.AddOrUpdateAirportInfoAsync(IATACode, airportInformation, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new AirportInformationServiceException("Add or update airport error", ex);
            }
        }
    }
}

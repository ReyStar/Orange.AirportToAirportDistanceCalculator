using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Orange.AirportToAirportDistanceCalculator.Application.Interfaces;
using Orange.AirportToAirportDistanceCalculator.Domain.Exceptions;
using Orange.AirportToAirportDistanceCalculator.Domain.Interfaces;
using Orange.AirportToAirportDistanceCalculator.Domain.Models;
using Polly.CircuitBreaker;
using Refit;

namespace Orange.AirportToAirportDistanceCalculator.Application.Services
{
    /// <summary>
    ///  A service that provides information about airports
    /// </summary>
    class AirportInformationProviderService : IAirportInformationProviderService
    {
        private readonly ICteleportClient _cteleportClient;
        private readonly IMapper _mapper;

        /// <summary>Initializes a new instance of the <see cref="AirportInformationProviderService" /> class.</summary>
        /// <param name="cteleportClient">The cteleport client.</param>
        /// <param name="mapper">The mapper.</param>
        public AirportInformationProviderService(ICteleportClient cteleportClient,
                                                 IMapper mapper)
        {
            _cteleportClient = cteleportClient;
            _mapper = mapper;
        }

        /// <summary>
        /// Get airport information using IATA.
        /// </summary>
        /// <param name="IATACode">The IATA code.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task<AirportInformation?> GetInformationAsync(string IATACode, CancellationToken cancellationToken)
        {
            try
            {
                var cteleportAirportInfo = await _cteleportClient.GetAirportInfoAsync(IATACode, cancellationToken);

                return _mapper.Map<AirportInformation>(cteleportAirportInfo);
            }
            catch (ValidationApiException ex)
            {
                throw new AirportInformationProviderException(ex);
            }
            catch (ApiException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            catch (ApiException ex)
            {
                throw new AirportInformationProviderException(ex);
            }
            catch (BrokenCircuitException ex)
            {
                throw new AirportInformationProviderException(ex);
            }
            catch (HttpRequestException ex)
            {
                throw new AirportInformationProviderException(ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new AirportInformationProviderException(ex);
            }
        }
    }
}

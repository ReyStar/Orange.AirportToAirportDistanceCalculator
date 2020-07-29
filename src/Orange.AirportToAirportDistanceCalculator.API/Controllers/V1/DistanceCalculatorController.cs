using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Orange.AirportToAirportDistanceCalculator.API.Attributes;
using Orange.AirportToAirportDistanceCalculator.API.Controllers.V1.DTO;
using Orange.AirportToAirportDistanceCalculator.Domain.Interfaces;

namespace Orange.AirportToAirportDistanceCalculator.API.Controllers.V1
{
    [RequireHttps]
    [ApiController]
    [VersionRoute("distances")]
    [NullModelValidation]
    [ValidateModel]
    [WebApiVersion(ApiVersions.V1)]
    public class DistanceCalculatorController : ControllerBase
    {
        private readonly IDistanceCalculatorService _distanceCalculatorService;
        private readonly IMapper _mapper;
        
        public DistanceCalculatorController(IDistanceCalculatorService distanceCalculatorService, 
                                            IMapper mapper)
        {
            _distanceCalculatorService = distanceCalculatorService;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns the distance between two airports.
        /// </summary>
        /// <param name="departureAirportIATACode">Departure airport IATA airport code is a three-letter geocode</param>
        /// <param name="destinationAirportIATACode">Destination airport IATA airport code is a three-letter geocode</param>
        /// <param name="cancellationToken">token for request canceling</param>
        [HttpGet]
        [ProducesResponseType(typeof(GeoDistanceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<IActionResult> GetDistancesAsync([FromQuery(Name = "from"), IATACodeValidation,
                                                            Required(AllowEmptyStrings = false)]
                                                            string departureAirportIATACode,
                                                           [FromQuery(Name = "to"),
                                                            Required(AllowEmptyStrings = false),
                                                            IATACodeValidation]
                                                           string destinationAirportIATACode,
                                                            CancellationToken cancellationToken)
        {
            var result = await _distanceCalculatorService.CalculateDistanceAsync(departureAirportIATACode, destinationAirportIATACode, cancellationToken);

            return Ok(_mapper.Map<GeoDistanceResponse>(result));
        }
    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AirportInfo.API.Attributes;
using AirportInfo.API.Controllers.V1.DTO;
using AirportInfo.Domain.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AirportInfo.API.Controllers.V1
{
    /// <summary>
    /// Search airport information
    /// </summary>
    [RequireHttps]
    [ApiController]
    [VersionRoute("airports")]
    [NullModelValidation]
    [ValidateModel]
    [WebApiVersion(ApiVersions.V1)]
    public class AirportsController : ControllerBase
    {
       private readonly IAirportInfoService _airportInfoService;
       private readonly IMapper _mapper;
        
        public AirportsController(IAirportInfoService airportInfoService, 
                                 IMapper mapper)
        {
           _airportInfoService = airportInfoService;
           _mapper = mapper;
        }

        /// <summary>
        /// Returns airport information by IATA
        /// </summary>/
        /// <param name="airportIATACode">Airport IATA code is a three-letter geocode</param>
        /// <param name="cancellationToken">token for request canceling</param>
        [HttpGet]
        [Route("{iata}")]
        [ProducesResponseType(typeof(AirportInfoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<IActionResult> GetAirportInfoAsync([FromRoute(Name = "iata"),
                                                              IATACodeValidation,
                                                              Required(AllowEmptyStrings = false)]
                                                              string airportIATACode,
                                                              CancellationToken cancellationToken = default)
        {
            var result = await _airportInfoService.GetAirportInfoAsync(airportIATACode, cancellationToken);

            return Ok(_mapper.Map<AirportInfoResponse>(result));
        }



        /// <summary>
        /// Finds the airport information use full text search
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AirportInfoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<IActionResult> FindAirportInfoByFuzzyStringAsync([FromQuery(Name = "query"),
                                                                            RegularExpression(@"\d*\w*\s*"),
                                                                            StringLength(1024, MinimumLength = 3),
                                                                            Required(AllowEmptyStrings = false)]
                                                                            string query,
                                                                            CancellationToken cancellationToken= default)
        {
            var result = await _airportInfoService.FindAirportInfoUseFullTextSearchAsync(query, cancellationToken);
            if (result.Any())
            {
                return Ok(_mapper.Map<IEnumerable<AirportInfoResponse>>(result));
            }

            return NotFound();
        }
    }
}

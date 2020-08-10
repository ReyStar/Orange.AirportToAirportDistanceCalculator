using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Routes.API.Attributes;
using Routes.API.Controllers.V1.DTO;
using Routes.Domain.Interfaces;
using Routes.Domain.Models;

namespace Routes.API.Controllers.V1
{
    /// <summary>
    /// Search route information
    /// </summary>
    [Authorize]
    [RequireHttps]
    [ApiController]
    [VersionRoute("user/{user-id}/routes")]
    [NullModelValidation]
    [ValidateModel]
    [WebApiVersion(ApiVersions.V1)]
    [UserAccess]
    public class RoutesController : ControllerBase
    {
       private readonly IRoutesService _routesService;
       private readonly IMapper _mapper;
        
        public RoutesController(IRoutesService routesService, 
                                IMapper mapper)
        {
           _routesService = routesService;
           _mapper = mapper;
        }

        /// <summary>
        /// Returns route information by user id and route id
        /// </summary>/
        /// <param name="route-id">Route id</param>
        /// <param name="cancellationToken">token for request canceling</param>
        [HttpGet]
        [Route("{route-id}")]
        [ProducesResponseType(typeof(RouteResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<IActionResult> GetRouteAsync([FromRoute(Name = "user-id"), Required] Guid userId,
                                                       [FromRoute(Name = "route-id"), Required] Guid routeId,
                                                        CancellationToken cancellationToken = default)
        {
            var result = await _routesService.GetRouteAsync(routeId, userId, cancellationToken);
            if (result != null)
            { 
                return Ok(_mapper.Map<RouteResponse>(result));
            }
            
            return NotFound();
        }

        /// <summary>
        /// Returns route information by user id and route id
        /// </summary>/
        /// <param name="cancellationToken">token for request canceling</param>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RouteResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<IActionResult> GetRoutesAsync([FromRoute(Name = "user-id"), Required] Guid userId, 
                                                        CancellationToken cancellationToken = default)
        {
            var result = await _routesService.GetRoutesAsync(userId, cancellationToken);
            if (result!=null && result.Any())
            {
                return Ok(_mapper.Map<IEnumerable<RouteResponse>>(result));
            }

            return NotFound();
        }


        /// <summary>Adds the or update route</summary>
        /// <param name="routeRequest">The route request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<IActionResult> AddOrUpdateRouteAsync([FromRoute(Name = "user-id"), Required] Guid userId, 
                                                               [Required] RouteRequest routeRequest,
                                                               CancellationToken cancellationToken = default)
        {
            var route = _mapper.Map<Route>(routeRequest);

            await _routesService.AddOrUpdateRouteAsync(route, userId, cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Delete route information by user id and route id
        /// </summary>/
        /// <param name="route-id">Route id</param>
        /// <param name="cancellationToken">token for request canceling</param>
        [HttpDelete]
        [Route("{route-id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<IActionResult> DeleteRouteAsync([FromRoute(Name = "user-id"), Required] Guid userId,
                                                          [FromRoute(Name = "route-id"), Required] Guid routeId,
                                                          CancellationToken cancellationToken = default)
        {
            await _routesService.DeleteRouteAsync(routeId, userId, cancellationToken);

            return NoContent();
        }
    }
}

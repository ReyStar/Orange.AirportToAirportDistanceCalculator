using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Routes.Domain.Models;

namespace Routes.Domain.Interfaces
{
    /// <summary>
    /// Service for add and return route information
    /// </summary>
    public interface IRoutesService
    {
        /// <summary>
        /// Get route by route id and user id
        /// </summary>
        Task<Route> GetRouteAsync(Guid routeId, Guid userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get all routes by user id
        /// </summary>
        Task<IEnumerable<Route>> GetRoutesAsync(Guid userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Add or update route
        /// </summary>
        Task AddOrUpdateRouteAsync(Route route, Guid userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes the route
        /// </summary>
        Task DeleteRouteAsync(Guid routeId, Guid userId, CancellationToken cancellationToken = default);
    }
}

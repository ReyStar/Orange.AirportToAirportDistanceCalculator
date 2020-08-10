using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Routes.Domain.Models;

namespace Routes.Domain.Interfaces
{
    /// <summary>
    /// Repository for storing routes
    /// </summary>
    public interface IRoutesRepository
    {
        /// <summary>
        /// Get route by route id and user id
        /// </summary>
        Task<Route> GetRouteAsync(Guid routeId, Guid userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get routes by user id
        /// </summary>
        Task<IEnumerable<Route>> GetRoutesAsync(Guid userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds the or update route.
        /// </summary>
        Task<bool> AddOrUpdateRouteAsync(Route route, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes the route
        /// </summary>
        Task<bool> DeleteRouteAsync(Guid routeId, Guid userId, CancellationToken cancellationToken = default);
    }
}

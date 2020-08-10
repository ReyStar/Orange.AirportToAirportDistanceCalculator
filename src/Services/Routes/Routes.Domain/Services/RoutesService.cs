using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Routes.Domain.Exceptions;
using Routes.Domain.Interfaces;
using Routes.Domain.Models;

namespace Routes.Domain.Services
{
    /// <summary>
    /// A service that provides routes information
    /// </summary>
    class RoutesService : IRoutesService
    {
        private readonly IRoutesRepository _routesRepository;

        public RoutesService(IRoutesRepository routesRepository)
        {
            _routesRepository = routesRepository;
        }

        public async Task<Route> GetRouteAsync(Guid routeId, Guid userId, CancellationToken cancellationToken = default)
        {
           return await _routesRepository.GetRouteAsync(routeId, userId, cancellationToken);
        }

        public async Task<IEnumerable<Route>> GetRoutesAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _routesRepository.GetRoutesAsync(userId, cancellationToken);
        }

        public async Task AddOrUpdateRouteAsync(Route route, Guid userId, CancellationToken cancellationToken = default)
        {
            try
            {
                route.UserId = userId;

                await _routesRepository.AddOrUpdateRouteAsync(route, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new RouteServiceException("Add or update airport error", ex);
            }
        }

        public async Task DeleteRouteAsync(Guid routeId, Guid userId, CancellationToken cancellationToken = default)
        {
            await _routesRepository.DeleteRouteAsync(routeId, userId, cancellationToken);
        }
    }
}

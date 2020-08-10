using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Routes.Domain.Interfaces;
using Routes.Domain.Models;

namespace Routes.Application.Services
{
    /// <summary>
    /// In memory caching airport information for the Routes
    /// </summary>
    class RoutesMemoryCacheService : IRoutesService
    {
        private readonly IRoutesService _routesService;
        private readonly IMemoryCache _memoryCache;
        private readonly MemoryCacheEntryOptions _memoryCacheGeoDistanceEntryOptions;

        public RoutesMemoryCacheService(IRoutesService routesService, IMemoryCache memoryCache)
        {
            _routesService = routesService;
            _memoryCache = memoryCache;
            
            _memoryCacheGeoDistanceEntryOptions = new MemoryCacheEntryOptions
                                                  {
                                                      Size = 256
                                                  };
        }

        public Task<Route> GetRouteAsync(Guid routeId, Guid userId, CancellationToken cancellationToken = default)
        {
            return _routesService.GetRouteAsync(routeId, userId, cancellationToken);
        }

        public Task<IEnumerable<Route>> GetRoutesAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return _routesService.GetRoutesAsync(userId, cancellationToken);
        }

        public Task AddOrUpdateRouteAsync(Route route, Guid userId, CancellationToken cancellationToken = default)
        {
            return _routesService.AddOrUpdateRouteAsync(route, userId, cancellationToken);
        }

        public Task DeleteRouteAsync(Guid routeId, Guid userId, CancellationToken cancellationToken = default)
        {
            return _routesService.DeleteRouteAsync(routeId, userId, cancellationToken);
        }
    }
}

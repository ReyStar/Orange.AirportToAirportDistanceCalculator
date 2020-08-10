using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MongoDB.Driver;
using Routes.Domain.Exceptions;
using Routes.Domain.Interfaces;
using Routes.Domain.Models;
using Routes.Repository.Models;

namespace Routes.Repository.Repositories
{
    /// <summary>
    /// Repository for storing routes information
    /// </summary>
    internal class RoutesRepository: IRoutesRepository
    {
        private readonly IDataSource _dataSource;
        private readonly IMapper _mapper;
        private readonly IMongoCollection<RouteDbModel> _mongoCollection;

        public const string CollectionName = "Route";

        public RoutesRepository(IDataSource dataSource, IMapper mapper)
        {
            _dataSource = dataSource;
            _mapper = mapper;
            _mongoCollection = _dataSource.Database.GetCollection<RouteDbModel>(CollectionName);

        }

        public async Task<Route> GetRouteAsync(Guid routeId, Guid userId, CancellationToken cancellationToken = default)
        {
            //var t = await _mongoCollection
            //    .FindAsync(x => x.Id == routeId && x.UserId == userId,
            //    cancellationToken: cancellationToken);
            try
            {
                var cursor = _mongoCollection.Find(x => x.Id == routeId && x.UserId == userId);
                var airportInformationDbModel = await cursor.FirstOrDefaultAsync(cancellationToken);

                return _mapper.Map<Route>(airportInformationDbModel);
            }
            catch (Exception ex)
            {
                throw new RouteServiceException("Get route error", ex);
            }
        }

        public async Task<IEnumerable<Route>> GetRoutesAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            try
            {
                var cursor = _mongoCollection.Find(x => x.UserId == userId);
                var airportInformationDbModel = await cursor.ToListAsync(cancellationToken);

                return _mapper.Map<IEnumerable<Route>>(airportInformationDbModel);
            }
            catch (Exception ex)
            {
                throw new RouteServiceException("Get route error", ex);
            }
        }

        public async Task<bool> AddOrUpdateRouteAsync(Route route, CancellationToken cancellationToken = default)
        {
            try
            {
                var dbModel = _mapper.Map<RouteDbModel>(route);
                if (Guid.Empty == dbModel.Id)
                {
                    dbModel.Id = Guid.NewGuid();
                }

                var result = await _mongoCollection.ReplaceOneAsync(x => x.Id == dbModel.Id, dbModel,
                                                                    new ReplaceOptions
                                                                    {
                                                                        IsUpsert = true
                                                                    }, cancellationToken: cancellationToken);

                return result.IsModifiedCountAvailable;

            }
            catch (Exception ex)
            {
                throw new RouteServiceException("Add or update route error", ex);
            }
        }

        public async Task<bool> DeleteRouteAsync(Guid routeId, Guid userId, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _mongoCollection.DeleteManyAsync(x => x.Id == routeId && x.UserId == userId,
                                                                    cancellationToken: cancellationToken);
                return result.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                throw new RouteServiceException("Delete route error", ex);
            }
        }
    }
}

using AutoMapper;
using Routes.Repository.DataBaseProducer.Migrations;
using Routes.Repository.Models;

namespace Routes.Repository
{
    class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Domain.Models.Route, RouteDbModel>();
            CreateMap<RouteDbModel, Domain.Models.Route>();

            CreateMap<Domain.Models.RoutePoint, RoutePointDbModel>();
            CreateMap<RoutePointDbModel, Domain.Models.RoutePoint>();

            CreateMap<Domain.Models.GeoCoordinate, GeoCoordinateDbModel>();
            CreateMap<GeoCoordinateDbModel, Domain.Models.GeoCoordinate>();
        }
    }
}

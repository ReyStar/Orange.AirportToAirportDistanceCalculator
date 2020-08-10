using AutoMapper;
using Routes.API.Controllers.V1.DTO;

namespace Routes.API
{
    class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Domain.Models.Route, RouteResponse>();
            CreateMap<RouteResponse, Domain.Models.Route>();

            CreateMap<Domain.Models.Route, RouteRequest>();
            CreateMap<RouteRequest, Domain.Models.Route>();

            CreateMap<Domain.Models.RoutePoint, RoutePoint>();
            CreateMap<RoutePoint, Domain.Models.RoutePoint>();

            CreateMap<Domain.Models.GeoCoordinate, GeoCoordinate>();
            CreateMap<GeoCoordinate, Domain.Models.GeoCoordinate>();
        }
    }
}

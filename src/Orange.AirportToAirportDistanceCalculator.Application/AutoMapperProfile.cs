using AutoMapper;
using Orange.AirportToAirportDistanceCalculator.Application.Models;
using Orange.AirportToAirportDistanceCalculator.Domain.Models;

namespace Orange.AirportToAirportDistanceCalculator.Application
{
    class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CteleportAirportInfo, AirportInformation>()
                .ForMember(p => p.IATA, x => x.MapFrom(c => c.IATA))
                .ForMember(p => p.Location, x => x.MapFrom(c => c.Location));

            CreateMap<CteleportGeoCoordinate, GeoCoordinate>();
        }
    }
}

using AutoMapper;
using DistanceCalculator.Application.Models;
using DistanceCalculator.Domain.Models;

namespace DistanceCalculator.Application
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

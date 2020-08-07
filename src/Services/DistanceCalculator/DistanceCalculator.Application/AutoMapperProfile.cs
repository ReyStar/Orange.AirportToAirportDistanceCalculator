using AutoMapper;

namespace DistanceCalculator.Application
{
    class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Models.AirportInfo, Domain.Models.AirportInformation>()
                .ForMember(p => p.IATA, x => x.MapFrom(c => c.IATACode));

            CreateMap<Models.GeoCoordinate, Domain.Models.GeoCoordinate>();
        }
    }
}

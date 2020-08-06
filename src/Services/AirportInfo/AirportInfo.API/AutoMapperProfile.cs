using AirportInfo.API.Controllers.V1.DTO;
using AutoMapper;

namespace AirportInfo.API
{
    class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Domain.Models.AirportInformation, AirportInfoResponse>();
            CreateMap<Domain.Models.GeoCoordinate, GeoCoordinateResponse>();
        }
    }
}

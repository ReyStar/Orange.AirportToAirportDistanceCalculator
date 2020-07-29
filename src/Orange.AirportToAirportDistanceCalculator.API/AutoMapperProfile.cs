using AutoMapper;
using Orange.AirportToAirportDistanceCalculator.API.Controllers.V1.DTO;
using Orange.AirportToAirportDistanceCalculator.Domain.Models;

namespace Orange.AirportToAirportDistanceCalculator.API
{
    class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<GeoDistance, GeoDistanceResponse>();
        }
    }
}

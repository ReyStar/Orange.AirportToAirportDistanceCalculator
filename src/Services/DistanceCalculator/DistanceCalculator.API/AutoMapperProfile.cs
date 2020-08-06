using AutoMapper;
using DistanceCalculator.API.Controllers.V1.DTO;
using DistanceCalculator.Domain.Models;

namespace DistanceCalculator.API
{
    class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<GeoDistance, GeoDistanceResponse>();
        }
    }
}

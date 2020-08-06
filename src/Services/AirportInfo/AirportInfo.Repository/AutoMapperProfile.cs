using AirportInfo.Repository.DataBaseProducer.Migrations;
using AirportInfo.Repository.Models;
using AutoMapper;

namespace AirportInfo.Repository
{
    class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AirportImportModel, AirportInformationDbModel>();
            CreateMap<GeoCoordinateImportModel, GeoCoordinateDbModel>();

            CreateMap<Domain.Models.AirportInformation, AirportInformationDbModel>();
            CreateMap<AirportInformationDbModel, Domain.Models.AirportInformation>();

            CreateMap<Domain.Models.GeoCoordinate, GeoCoordinateDbModel>();
            CreateMap<GeoCoordinateDbModel, Domain.Models.GeoCoordinate>();
        }
    }
}

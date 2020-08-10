using AccountManager.API.Controllers.V1.DTO;
using AccountManager.Domain.Models;
using AutoMapper;

namespace AccountManager.API
{
    class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AuthenticateModel, AccountResponse>();
            
            CreateMap<RegisterAccountRequest, RegisterAccountModel>();
            
            CreateMap<Account, AccountResponse>();
            
            CreateMap<UpdateAccountRequest, UpdateAccountModel>();

            CreateMap<AuthenticateModel, LoginResponse>();

        }
    }
}

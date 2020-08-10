using AccountManager.Domain.Models;
using AutoMapper;

namespace AccountManager.Domain
{
    class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegisterAccountModel, AccountInfo>()
                .ForMember(x => x.PasswordSalt, y => y.Ignore())
                .ForMember(x => x.PasswordHash, y => y.Ignore())
                .ForMember(x => x.Id, y => y.Ignore());

            CreateMap<AccountInfo, AuthenticateModel>()
                .ForMember(x => x.AccessToken, y => y.Ignore());

            CreateMap<UpdateAccountModel, AccountInfo>()
                .ForMember(x => x.PasswordSalt, y => y.Ignore())
                .ForMember(x => x.PasswordHash, y => y.Ignore());
        }
    }
}

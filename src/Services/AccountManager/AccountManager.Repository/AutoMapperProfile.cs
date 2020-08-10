using AccountManager.Domain.Models;
using AccountManager.Repository.Models;
using AutoMapper;

namespace AccountManager.Repository
{
    class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AccountInfo, AccountDbModel>()
                .ForMember(x => x.IsDeleted, y => y.Ignore());
            CreateMap<AccountDbModel, AccountInfo>();

            CreateMap<RefreshToken, RefreshTokenDbModel>()
                .ForMember(x => x.IsDeleted, y => y.Ignore());
            CreateMap<RefreshTokenDbModel, RefreshToken>();

            CreateMap<AccessToken, AccessTokenDbModel>()
                .ForMember(x => x.IsDeleted, y => y.Ignore());
            CreateMap<AccessTokenDbModel, AccessToken>();
        }
    }
}

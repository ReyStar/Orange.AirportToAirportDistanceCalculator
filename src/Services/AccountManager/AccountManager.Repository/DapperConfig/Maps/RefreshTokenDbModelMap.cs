using AccountManager.Repository.Models;
using Dapper.Dommel.FluentMapping;

namespace AccountManager.Repository.DapperConfig.Maps
{
    class RefreshTokenDbModelMap : DommelEntityMap<RefreshTokenDbModel>
    {
        public RefreshTokenDbModelMap()
        {
            ToTable("refreshtoken");

            Map(x => x.Token).ToColumn("token").IsKey();

            Map(x => x.Expires).ToColumn("expires");

            Map(x => x.Created).ToColumn("created");
            
            Map(x => x.AccountId).ToColumn("account_id");

            Map(x => x.IsDeleted).ToColumn("is_deleted");
        }
    }
}

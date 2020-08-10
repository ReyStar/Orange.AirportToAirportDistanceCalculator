using AccountManager.Repository.Models;
using Dapper.Dommel.FluentMapping;

namespace AccountManager.Repository.DapperConfig.Maps
{
    class AccessTokenDbModelMap : DommelEntityMap<AccessTokenDbModel>
    {
        public AccessTokenDbModelMap()
        {
            ToTable("accesstoken");

            Map(x => x.TokenHash).ToColumn("token_hash").IsKey();

            Map(x => x.Token).ToColumn("token");

            Map(x => x.Expires).ToColumn("expires");

            Map(x => x.Created).ToColumn("created");
            
            Map(x => x.AccountId).ToColumn("account_id");

            Map(x => x.IsDeleted).ToColumn("is_deleted");
        }
    }
}

using AccountManager.Repository.Models;
using Dapper.Dommel.FluentMapping;

namespace AccountManager.Repository.DapperConfig.Maps
{
    class VersionInfoDbModelMap : DommelEntityMap<VersionInfoDbModel>
    {
        public VersionInfoDbModelMap()
        {
            ToTable("VersionInfo");

            Map(x => x.Version).ToColumn("Version").IsKey();

            Map(x => x.AppliedOn).ToColumn("AppliedOn");

            Map(x => x.Description).ToColumn("Description");
        }
    }
}

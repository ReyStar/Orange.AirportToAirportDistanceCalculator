using AccountManager.Repository.Models;
using Dapper.Dommel.FluentMapping;

namespace AccountManager.Repository.DapperConfig.Maps
{
    class AccountDbModelMap : DommelEntityMap<AccountDbModel>
    {
        //IsKey - this property used by Dommel for control insert or update functions.
        //This orm can use multi column key. We can use natural Id key (concatenation two or more columns)
        //or create some unique key in column, but this key will end up in the domain.
        public AccountDbModelMap()
        {
            ToTable("account");
            
            Map(x => x.Id).ToColumn("id").IsKey();
            
            Map(x => x.Username).ToColumn("username");
            
            Map(x => x.FirstName).ToColumn("firstname");
            
            Map(x => x.LastName).ToColumn("lastname");
            
            Map(x => x.PasswordHash).ToColumn("password_hash");
            
            Map(x => x.PasswordSalt).ToColumn("password_salt");
            
            Map(x => x.Revision).ToColumn("revision");
            
            Map(x => x.IsDeleted).ToColumn("is_deleted");
        }
    }
}

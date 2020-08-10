using FluentMigrator;

namespace AccountManager.Repository.DataBaseProducer.Migrations
{
    /// <summary>
    /// Migration to create account table
    /// </summary>
    [Migration(3, "CreateAsync refreshtoken table")]
    public class RefreshTokenTableInitializeMigration : ForwardOnlyMigration
    {
        private const string TableName = "refreshtoken";

        public override void Up()
        {
            Create
                .Table(TableName)
                .WithColumn("token").AsAnsiString(92).NotNullable().PrimaryKey("PK_refresh_token")
                .WithColumn("account_id").AsGuid().NotNullable()
                .WithColumn("expires").AsDateTime().NotNullable()
                .WithColumn("created").AsDateTime().NotNullable()
                .WithColumn("is_deleted").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("revision").AsInt32().NotNullable().WithDefaultValue(1) //TODO add trigger with increment on update
                .WithColumn("server_time").AsDateTime2().WithDefault(SystemMethods.CurrentUTCDateTime);

            //update server_time use sql server instance time
            Execute.Sql("CREATE OR REPLACE FUNCTION trigger_refreshtoken_set_timestamp() RETURNS TRIGGER AS $$ BEGIN NEW.server_time = NOW(); RETURN NEW; END; $$ LANGUAGE plpgsql; ");
            Execute.Sql($"CREATE TRIGGER set_timestamp BEFORE UPDATE ON {TableName} FOR EACH ROW EXECUTE PROCEDURE trigger_refreshtoken_set_timestamp(); ");
        }
    }
}

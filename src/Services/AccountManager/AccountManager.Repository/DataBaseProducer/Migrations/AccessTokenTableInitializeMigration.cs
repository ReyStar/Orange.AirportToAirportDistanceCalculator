using FluentMigrator;

namespace AccountManager.Repository.DataBaseProducer.Migrations
{
    /// <summary>
    /// Migration to create accesstoken table
    /// </summary>
    [Migration(2, "CreateAsync accesstoken table")]
    public class AccessTokenTableInitializeMigration : ForwardOnlyMigration
    {
        private const string TableName = "accesstoken";

        public override void Up()
        {
            Create
                .Table(TableName)
                .WithColumn("token_hash").AsAnsiString(64).NotNullable().PrimaryKey("PK_access_token_hash")
                .WithColumn("token").AsAnsiString(256).Nullable()
                .WithColumn("account_id").AsGuid().NotNullable()
                .WithColumn("expires").AsDateTime().NotNullable()
                .WithColumn("created").AsDateTime().NotNullable()
                .WithColumn("is_deleted").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("revision").AsInt32().NotNullable().WithDefaultValue(1) //TODO add trigger with increment on update
                .WithColumn("server_time").AsDateTime2().WithDefault(SystemMethods.CurrentUTCDateTime);

            //update server_time use sql server instance time
            Execute.Sql("CREATE OR REPLACE FUNCTION trigger_accesstoken_set_timestamp() RETURNS TRIGGER AS $$ BEGIN NEW.server_time = NOW(); RETURN NEW; END; $$ LANGUAGE plpgsql; ");
            Execute.Sql($"CREATE TRIGGER set_timestamp BEFORE UPDATE ON {TableName} FOR EACH ROW EXECUTE PROCEDURE trigger_accesstoken_set_timestamp(); ");
        }
    }
}

using FluentMigrator;

namespace AccountManager.Repository.DataBaseProducer.Migrations
{
    /// <summary>
    /// Migration to create account table
    /// </summary>
    [Migration(1, "CreateAsync account table")]
    public class AccountTableInitializeMigration : ForwardOnlyMigration
    {
        private const string TableName = "account";

        public override void Up()
        {
            Create
                .Table(TableName)
                .WithColumn("id").AsGuid().NotNullable().PrimaryKey("PK_account_id")
                .WithColumn("username").AsString(256).NotNullable().Unique("UC_username")
                .WithColumn("firstname").AsString(256).NotNullable()
                .WithColumn("lastname").AsString(256).NotNullable()
                .WithColumn("password_hash").AsBinary(64).NotNullable()
                .WithColumn("password_salt").AsBinary(128).NotNullable()
                .WithColumn("revision").AsInt32().NotNullable().WithDefaultValue(1) //TODO add trigger with increment on update
                .WithColumn("is_deleted").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("server_time").AsDateTime2().WithDefault(SystemMethods.CurrentUTCDateTime);

            //update server_time use sql server instance time
            Execute.Sql("CREATE OR REPLACE FUNCTION trigger_set_timestamp() RETURNS TRIGGER AS $$ BEGIN NEW.server_time = NOW(); RETURN NEW; END; $$ LANGUAGE plpgsql; ");
            Execute.Sql($"CREATE TRIGGER set_timestamp BEFORE UPDATE ON {TableName} FOR EACH ROW EXECUTE PROCEDURE trigger_set_timestamp(); ");
        }
    }
}

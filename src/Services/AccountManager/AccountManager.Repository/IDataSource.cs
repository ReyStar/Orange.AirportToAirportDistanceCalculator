using System.Data;

namespace AccountManager.Repository
{
    internal interface IDataSource
    {
        IDbConnection Connection { get; }
    }
}

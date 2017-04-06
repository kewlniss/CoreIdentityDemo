using System.Data;

namespace CoreIdentityDemo.Data.DapperSql.Repositories
{
    internal abstract class BaseRepository
    {
        public BaseRepository(IDbTransaction transaction)
        {
            Transaction = transaction;
        }

        protected IDbTransaction Transaction { get; private set; }
        protected IDbConnection Connection { get { return Transaction.Connection; } }
    }
}

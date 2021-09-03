using System.Data;
using Wyn.Data.Abstractions;
using Wyn.Data.Abstractions.Adapter;

namespace Wyn.Data.Core
{
    internal class UnitOfWork : IUnitOfWork
    {
        public IDbTransaction Transaction { get; private set; }

        public UnitOfWork(IDbContext dbContext, IsolationLevel? isolationLevel)
        {
            //SQLite数据库开启事务时会报 database is locked 错误，所以无法使用工作单元
            if (dbContext.Adapter.Provider == DbProvider.Sqlite)
                return;

            var con = dbContext.NewConnection();
            con.Open();
            Transaction = isolationLevel != null ? con.BeginTransaction(isolationLevel.Value) : con.BeginTransaction();
        }

        public void SaveChanges()
        {
            if (Transaction != null)
            {
                Transaction.Commit();
                Transaction.Connection?.Close();
                Transaction = null;
            }
        }

        public void Rollback()
        {
            if (Transaction != null)
            {
                Transaction.Rollback();
                Transaction.Connection?.Close();
                Transaction = null;
            }
        }

        public void Dispose()
        {
            Rollback();
        }
    }
}

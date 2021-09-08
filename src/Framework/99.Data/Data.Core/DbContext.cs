using System.Collections.Generic;
using System.Data;

using Wyn.Data.Abstractions;
using Wyn.Data.Abstractions.Adapter;
using Wyn.Data.Abstractions.Descriptors;
using Wyn.Data.Abstractions.Logger;
using Wyn.Data.Abstractions.Options;
using Wyn.Data.Abstractions.Schema;

namespace Wyn.Data.Core
{
    public abstract class DbContext : IDbContext
    {
        #region 属性

        public DbOptions Options { get; internal set; }

        public DbLogger Logger { get; internal set; }

        public IDbAdapter Adapter { get; internal set; }

        public ISchemaProvider SchemaProvider { get; internal set; }

        public ICodeFirstProvider CodeFirstProvider { get; internal set; }

        public IAccountResolver AccountResolver { get; internal set; }

        public IList<IEntityDescriptor> EntityDescriptors { get; } = new List<IEntityDescriptor>();

        public IList<IRepositoryDescriptor> RepositoryDescriptors { get; } = new List<IRepositoryDescriptor>();

        #endregion

        #region 方法

        public IDbConnection NewConnection() => Adapter.NewConnection(Options.ConnectionString);

        public IDbConnection NewConnection(string connectionString) => Adapter.NewConnection(connectionString);

        public IUnitOfWork NewUnitOfWork(IsolationLevel? isolationLevel = null) => new UnitOfWork(this, isolationLevel);

        #endregion
    }
}

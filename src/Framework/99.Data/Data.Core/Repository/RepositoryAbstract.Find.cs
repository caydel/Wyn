using System;
using System.Linq.Expressions;

using Wyn.Data.Abstractions.Queryable;
using Wyn.Data.Core.Queryable;

namespace Wyn.Data.Core.Repository
{
    public abstract partial class RepositoryAbstract<TEntity>
    {
        public IQueryable<TEntity> Find() => new Queryable<TEntity>(this, null, true);

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> expression) => new Queryable<TEntity>(this, expression, true);

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> expression, bool noLock) => new Queryable<TEntity>(this, expression, noLock);
    }
}

using System;
using System.Linq.Expressions;

using Wyn.Data.Abstractions.Entities;
using Wyn.Data.Abstractions.Logger;
using Wyn.Data.Abstractions.Pagination;
using Wyn.Data.Abstractions.Queryable.Grouping;
using Wyn.Data.Core.SqlBuilder;

namespace Wyn.Data.Core.Queryable.Grouping
{
    internal class GroupingQueryable<TKey, TEntity> : GroupingQueryable, IGroupingQueryable<TKey, TEntity> where TEntity : IEntity, new()
    {
        public GroupingQueryable(QueryableSqlBuilder sqlBuilder, DbLogger logger, Expression expression) : base(sqlBuilder, logger, expression)
        {
        }

        public IGroupingQueryable<TKey, TEntity> Having(Expression<Func<Abstractions.Queryable.Grouping.IGrouping<TKey, TEntity>, bool>> expression)
        {
            _queryBody.SetHaving(expression);
            return this;
        }

        public IGroupingQueryable<TKey, TEntity> Having(string havingSql)
        {
            _queryBody.SetHaving(havingSql);
            return this;
        }

        public IGroupingQueryable<TKey, TEntity> OrderBy(string field)
        {
            _queryBody.SetSort(field, SortType.Asc);
            return this;
        }

        public IGroupingQueryable<TKey, TEntity> OrderByDescending(string field)
        {
            _queryBody.SetSort(field, SortType.Desc);
            return this;
        }

        public IGroupingQueryable<TKey, TEntity> OrderBy<TResult>(Expression<Func<System.Linq.IGrouping<TKey, TEntity>, TResult>> expression)
        {
            _queryBody.SetSort(expression, SortType.Asc);
            return this;
        }

        public IGroupingQueryable<TKey, TEntity> OrderByDescending<TResult>(Expression<Func<System.Linq.IGrouping<TKey, TEntity>, TResult>> expression)
        {
            _queryBody.SetSort(expression, SortType.Desc);
            return this;
        }

        public IGroupingQueryable<TKey, TEntity> Select<TResult>(Expression<Func<System.Linq.IGrouping<TKey, TEntity>, TResult>> expression)
        {
            _queryBody.SetSelect(expression);
            return this;
        }

        IGroupingQueryable<TKey, TEntity> IGroupingQueryable<TKey, TEntity>.OrderBy<TResult>(Expression<Func<Abstractions.Queryable.Grouping.IGrouping<TKey, TEntity>, TResult>> expression)
        {
            throw new NotImplementedException();
        }

        IGroupingQueryable<TKey, TEntity> IGroupingQueryable<TKey, TEntity>.OrderByDescending<TResult>(Expression<Func<Abstractions.Queryable.Grouping.IGrouping<TKey, TEntity>, TResult>> expression)
        {
            throw new NotImplementedException();
        }

        IGroupingQueryable<TKey, TEntity> IGroupingQueryable<TKey, TEntity>.Select<TResult>(Expression<Func<Abstractions.Queryable.Grouping.IGrouping<TKey, TEntity>, TResult>> expression)
        {
            throw new NotImplementedException();
        }
    }
}

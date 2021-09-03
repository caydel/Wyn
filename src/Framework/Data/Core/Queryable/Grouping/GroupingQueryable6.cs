using System;
using System.Linq.Expressions;
using Wyn.Data.Abstractions.Entities;
using Wyn.Data.Abstractions.Logger;
using Wyn.Data.Abstractions.Pagination;
using Wyn.Data.Abstractions.Queryable.Grouping;
using Wyn.Data.Core.SqlBuilder;

namespace Wyn.Data.Core.Queryable.Grouping
{
    internal class GroupingQueryable<TKey, TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6> : GroupingQueryable, IGroupingQueryable<TKey, TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6>
        where TEntity : IEntity, new()
        where TEntity2 : IEntity, new()
        where TEntity3 : IEntity, new()
        where TEntity4 : IEntity, new()
        where TEntity5 : IEntity, new()
        where TEntity6 : IEntity, new()
    {
        public GroupingQueryable(QueryableSqlBuilder sqlBuilder, DbLogger logger, Expression expression) : base(sqlBuilder, logger, expression)
        {
        }

        public IGroupingQueryable<TKey, TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6> Having(Expression<Func<IGrouping<TKey, TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6>, bool>> expression)
        {
            _queryBody.SetHaving(expression);
            return this;
        }

        public IGroupingQueryable<TKey, TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6> Having(string havingSql)
        {
            _queryBody.SetHaving(havingSql);
            return this;
        }

        public IGroupingQueryable<TKey, TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6> OrderBy(string field)
        {
            _queryBody.SetSort(field, SortType.Asc);
            return this;
        }

        public IGroupingQueryable<TKey, TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6> OrderByDescending(string field)
        {
            _queryBody.SetSort(field, SortType.Desc);
            return this;
        }

        public IGroupingQueryable<TKey, TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6> OrderBy<TResult>(Expression<Func<IGrouping<TKey, TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6>, TResult>> expression)
        {
            _queryBody.SetSort(expression, SortType.Asc);
            return this;
        }

        public IGroupingQueryable<TKey, TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6> OrderByDescending<TResult>(Expression<Func<IGrouping<TKey, TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6>, TResult>> expression)
        {
            _queryBody.SetSort(expression, SortType.Desc);
            return this;
        }

        public IGroupingQueryable<TKey, TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6> Select<TResult>(Expression<Func<IGrouping<TKey, TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6>, TResult>> expression)
        {
            _queryBody.SetSelect(expression);
            return this;
        }
    }
}

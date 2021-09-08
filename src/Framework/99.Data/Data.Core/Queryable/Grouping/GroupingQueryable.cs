using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Wyn.Data.Abstractions;
using Wyn.Data.Abstractions.Logger;
using Wyn.Data.Abstractions.Queryable;
using Wyn.Data.Abstractions.Queryable.Grouping;
using Wyn.Data.Core.Internal.QueryStructure;
using Wyn.Data.Core.SqlBuilder;

namespace Wyn.Data.Core.Queryable.Grouping
{
    internal abstract class GroupingQueryable : IGroupingQueryable
    {
        protected readonly QueryBody _queryBody;
        protected readonly GroupBySqlBuilder _sqlBuilder;
        protected readonly DbLogger _logger;
        protected readonly IRepository _repository;

        protected GroupingQueryable(QueryableSqlBuilder sqlBuilder, DbLogger logger, Expression expression)
        {
            _queryBody = sqlBuilder.QueryBody.Copy();
            _queryBody.IsGroupBy = true;
            _queryBody.SetGroupBy(expression);
            _sqlBuilder = new GroupBySqlBuilder(_queryBody);
            _logger = logger;
            _repository = _queryBody.Repository;
        }

        #region List

        public Task<IList<dynamic>> ToListDynamic() => ToList<dynamic>();

        public async Task<IList<TResult>> ToList<TResult>()
        {
            var sql = _sqlBuilder.BuildListSql(out IQueryParameters parameters);
            _logger.Write("GroupByToList", sql);
            return (await _repository.Query<TResult>(sql, parameters.ToDynamicParameters(), _queryBody.Uow)).ToList();
        }

        public string ToListSql() => _sqlBuilder.BuildListSql(out _);

        public string ToListSql(out IQueryParameters parameters) => _sqlBuilder.BuildListSql(out parameters);

        public string ToListSql(IQueryParameters parameters) => _sqlBuilder.BuildListSql(parameters);

        public string ToListSqlNotUseParameters() => _sqlBuilder.BuildListSqlNotUseParameters();

        #endregion

        #region Reader

        public Task<IDataReader> ToReader()
        {
            var sql = _sqlBuilder.BuildListSql(out IQueryParameters parameters);
            _logger.Write("GroupByToReader", sql);
            return _repository.ExecuteReader(sql, parameters.ToDynamicParameters(), _queryBody.Uow);
        }

        #endregion

        #region First

        public Task<dynamic> ToFirstDynamic() => ToFirst<dynamic>();

        public Task<TResult> ToFirst<TResult>()
        {
            var sql = _sqlBuilder.BuildFirstSql(out IQueryParameters parameters);
            _logger.Write("ToFirst", sql);
            return _repository.QueryFirstOrDefault<TResult>(sql, parameters.ToDynamicParameters(), _queryBody.Uow);
        }

        public string ToFirstSql() => _sqlBuilder.BuildFirstSql(out _);

        public string ToFirstSql(out IQueryParameters parameters) => _sqlBuilder.BuildFirstSql(out parameters);

        public string ToFirstSql(IQueryParameters parameters) => _sqlBuilder.BuildFirstSql(parameters);

        public string ToFirstSqlNotUseParameters() => _sqlBuilder.BuildFirstSqlNotUserParameters();

        #endregion
    }
}

using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Wyn.Data.Abstractions;
using Wyn.Data.Abstractions.Logger;
using Wyn.Data.Abstractions.Pagination;
using Wyn.Data.Abstractions.Query;
using Wyn.Data.Abstractions.Queryable;
using Wyn.Data.Core.Internal.QueryStructure;
using Wyn.Data.Core.SqlBuilder;
using Wyn.Utils.Result;

using IQueryable = Wyn.Data.Abstractions.Queryable.IQueryable;

namespace Wyn.Data.Core.Queryable
{
    internal class Queryable : IQueryable
    {
        protected readonly IRepository _repository;
        protected readonly QueryBody _queryBody;
        protected readonly QueryableSqlBuilder _sqlBuilder;
        protected readonly DbLogger _logger;

        public Queryable(IRepository repository)
        {
            _repository = repository;
            _logger = repository.DbContext.Logger;
            _queryBody = new QueryBody(repository);
            _sqlBuilder = new QueryableSqlBuilder(_queryBody);
        }

        protected Queryable(QueryBody queryBody)
        {
            _queryBody = queryBody;
            _repository = queryBody.Repository;
            _sqlBuilder = new QueryableSqlBuilder(_queryBody);
            _logger = _repository.DbContext.Logger;
        }

        #region List

        public Task<IList<dynamic>> ToListDynamic() => ToList<dynamic>();

        public async Task<IList<TResult>> ToList<TResult>()
        {
            var sql = _sqlBuilder.BuildListSql(out IQueryParameters parameters);
            _logger.Write("ToList", sql);
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
            _logger.Write("ToReader", sql);
            return _repository.ExecuteReader(sql, parameters.ToDynamicParameters(), _queryBody.Uow);
        }

        #endregion

        #region Pagination

        public Task<IList<dynamic>> ToPaginationDynamic() => ToPagination<dynamic>(null);

        public Task<IList<dynamic>> ToPaginationDynamic(Paging paging) => ToPagination<dynamic>(paging);

        public Task<IList<TResult>> ToPagination<TResult>() => ToPagination<TResult>(null);

        public async Task<IList<TResult>> ToPagination<TResult>(Paging paging)
        {
            if (paging == null)
                _queryBody.SetLimit(1, 15);
            else
                _queryBody.SetLimit(paging.Skip, paging.Size);

            var sql = _sqlBuilder.BuildPaginationSql(out IQueryParameters parameters);
            _logger.Write("ToPagination", sql);

            var task = _repository.Query<TResult>(sql, parameters.ToDynamicParameters(), _queryBody.Uow);

            if (paging != null && paging.QueryCount)
            {
                paging.TotalCount = await ToCount();
            }

            return (await task).ToList();
        }

        public async Task<IResultModel> ToPaginationResult<TResult>(Paging paging)
        {
            var rows = await ToPagination<TResult>(paging);
            return ResultModel.Success(new QueryResultModel<TResult>(rows, paging.TotalCount));
        }

        public string ToPaginationSql(Paging paging)
        {
            if (paging == null)
                _queryBody.SetLimit(1, 15);
            else
                _queryBody.SetLimit(paging.Skip, paging.Size);

            return _sqlBuilder.BuildPaginationSql(out _);
        }

        public string ToPaginationSql(Paging paging, out IQueryParameters parameters)
        {
            if (paging == null)
                _queryBody.SetLimit(1, 15);
            else
                _queryBody.SetLimit(paging.Skip, paging.Size);

            return _sqlBuilder.BuildPaginationSql(out parameters);
        }

        public string ToPaginationSql(Paging paging, IQueryParameters parameters)
        {
            if (paging == null)
                _queryBody.SetLimit(1, 15);
            else
                _queryBody.SetLimit(paging.Skip, paging.Size);

            return _sqlBuilder.BuildPaginationSql(parameters);
        }

        public string ToPaginationSqlNotUseParameters(Paging paging)
        {
            if (paging == null)
                _queryBody.SetLimit(1, 15);
            else
                _queryBody.SetLimit(paging.Skip, paging.Size);

            return _sqlBuilder.BuildPaginationSqlNotUseParameters();
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

        #region Count

        public Task<long> ToCount()
        {
            var sql = _sqlBuilder.BuildCountSql(out IQueryParameters parameters);
            _logger.Write("ToCount", sql);
            return _repository.ExecuteScalar<long>(sql, parameters.ToDynamicParameters(), _queryBody.Uow);
        }

        public string ToCountSql() => _sqlBuilder.BuildCountSql(out _);

        public string ToCountSql(out IQueryParameters parameters) => _sqlBuilder.BuildCountSql(out parameters);

        public string ToCountSql(IQueryParameters parameters) => _sqlBuilder.BuildCountSql(parameters);

        public string ToCountSqlNotUseParameters() => _sqlBuilder.BuildCountSqlNotUseParameters();

        #endregion

        #region Exists

        public async Task<bool> ToExists()
        {
            var sql = _sqlBuilder.BuildExistsSql(out IQueryParameters parameters);
            _logger.Write("ToExists", sql);
            return await _repository.ExecuteScalar<int>(sql, parameters.ToDynamicParameters(), _queryBody.Uow) > 0;
        }

        public string ToExistsSql() => _sqlBuilder.BuildExistsSql(out _);

        public string ToExistsSql(out IQueryParameters parameters) => _sqlBuilder.BuildExistsSql(out parameters);

        public string ToExistsSql(IQueryParameters parameters) => _sqlBuilder.BuildExistsSql(parameters);

        public string ToExistsSqlNotUseParameters() => _sqlBuilder.BuildExistsSqlNotUseParameters();

        #endregion

        #region Max

        protected Task<TResult> ToMax<TResult>(LambdaExpression expression) => ExecuteFunction<TResult>("Max", expression);

        public string MaxSql(LambdaExpression expression)
        {
            _queryBody.SetFunctionSelect(expression, "Max");
            return _sqlBuilder.BuildFunctionSql(out _);
        }

        public string MaxSql(LambdaExpression expression, out IQueryParameters parameters)
        {
            _queryBody.SetFunctionSelect(expression, "Max");
            return _sqlBuilder.BuildFunctionSql(out parameters);
        }

        public string MaxSql(LambdaExpression expression, IQueryParameters parameters)
        {
            _queryBody.SetFunctionSelect(expression, "Max");
            return _sqlBuilder.BuildFunctionSql(parameters);
        }

        public string MaxSqlNotUseParameters(LambdaExpression expression)
        {
            _queryBody.SetFunctionSelect(expression, "Max");
            return _sqlBuilder.BuildFunctionSqlNotUseParameters();
        }

        #endregion

        #region Min

        protected Task<TResult> ToMin<TResult>(LambdaExpression expression) => ExecuteFunction<TResult>("Min", expression);

        public string MinSql(LambdaExpression expression)
        {
            _queryBody.SetFunctionSelect(expression, "Min");
            return _sqlBuilder.BuildFunctionSql(out _);
        }

        public string MinSql(LambdaExpression expression, out IQueryParameters parameters)
        {
            _queryBody.SetFunctionSelect(expression, "Min");
            return _sqlBuilder.BuildFunctionSql(out parameters);
        }

        public string MinSql(LambdaExpression expression, IQueryParameters parameters)
        {
            _queryBody.SetFunctionSelect(expression, "Min");
            return _sqlBuilder.BuildFunctionSql(parameters);
        }

        public string MinSqlNotUseParameters(LambdaExpression expression)
        {
            _queryBody.SetFunctionSelect(expression, "Min");
            return _sqlBuilder.BuildFunctionSqlNotUseParameters();
        }

        #endregion

        #region Sum

        protected Task<TResult> ToSum<TResult>(LambdaExpression expression) => ExecuteFunction<TResult>("Sum", expression);

        public string SumSql(LambdaExpression expression)
        {
            _queryBody.SetFunctionSelect(expression, "Sum");
            return _sqlBuilder.BuildFunctionSql(out _);
        }

        public string SumSql(LambdaExpression expression, out IQueryParameters parameters)
        {
            _queryBody.SetFunctionSelect(expression, "Sum");
            return _sqlBuilder.BuildFunctionSql(out parameters);
        }

        public string SumSql(LambdaExpression expression, IQueryParameters parameters)
        {
            _queryBody.SetFunctionSelect(expression, "Sum");
            return _sqlBuilder.BuildFunctionSql(parameters);
        }

        public string SumSqlNotUseParameters(LambdaExpression expression)
        {
            _queryBody.SetFunctionSelect(expression, "Sum");
            return _sqlBuilder.BuildFunctionSqlNotUseParameters();
        }

        #endregion

        #region Avg

        protected Task<TResult> ToAvg<TResult>(LambdaExpression expression) => ExecuteFunction<TResult>("Avg", expression);

        public string AvgSql(LambdaExpression expression)
        {
            _queryBody.SetFunctionSelect(expression, "Avg");
            return _sqlBuilder.BuildFunctionSql(out _);
        }

        public string AvgSql(LambdaExpression expression, out IQueryParameters parameters)
        {
            _queryBody.SetFunctionSelect(expression, "Avg");
            return _sqlBuilder.BuildFunctionSql(out parameters);
        }

        public string AvgSql(LambdaExpression expression, IQueryParameters parameters)
        {
            _queryBody.SetFunctionSelect(expression, "Avg");
            return _sqlBuilder.BuildFunctionSql(parameters);
        }

        public string AvgSqlNotUseParameters(LambdaExpression expression)
        {
            _queryBody.SetFunctionSelect(expression, "Avg");
            return _sqlBuilder.BuildFunctionSqlNotUseParameters();
        }

        #endregion

        #region Function

        private Task<TResult> ExecuteFunction<TResult>(string functionName, LambdaExpression expression)
        {
            _queryBody.SetFunctionSelect(expression, functionName);
            var sql = _sqlBuilder.BuildFunctionSql(out IQueryParameters parameters);
            _logger.Write(functionName, sql);
            return _repository.ExecuteScalar<TResult>(sql, parameters.ToDynamicParameters(), _queryBody.Uow);
        }

        #endregion
    }
}

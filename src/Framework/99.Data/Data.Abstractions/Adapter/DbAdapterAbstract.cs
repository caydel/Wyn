using System;
using System.Data;
using System.Text;
using Wyn.Data.Abstractions.Descriptors;
using Wyn.Data.Abstractions.Options;

namespace Wyn.Data.Abstractions.Adapter
{
    /// <summary>
    /// 数据库适配器抽象类
    /// </summary>
    public abstract class DbAdapterAbstract : IDbAdapter
    {
        public abstract DbProvider Provider { get; }

        public DbOptions Options { get; set; }

        public virtual char LeftQuote => '"';

        public virtual char RightQuote => '"';

        public virtual char ParameterPrefix => '@';

        public virtual string IdentitySql => "";

        public virtual bool SqlLowerCase => false;

        public virtual string BooleanTrueValue => "1";

        public virtual string BooleanFalseValue => "0";

        public string AppendQuote(string value) => $"{LeftQuote}{(value?.Trim() != null && SqlLowerCase ? value?.Trim().ToLower() : value?.Trim())}{RightQuote}";

        public void AppendQuote(StringBuilder sb, string value) => sb.Append(AppendQuote(value));

        public string AppendParameter(string parameterName) => $"{ParameterPrefix}{parameterName}";

        public void AppendParameter(StringBuilder sb, string parameterName) => sb.Append(AppendParameter(parameterName));

        public abstract IDbConnection NewConnection(string connectionString);

        public abstract string GeneratePagingSql(string @select, string table, string @where, string sort, int skip, int take, string groupBy = null, string having = null);

        public abstract string GenerateFirstSql(string @select, string table, string @where, string sort, string groupBy = null, string having = null);

        public abstract void ResolveColumn(IColumnDescriptor columnDescriptor);

        public abstract string FunctionMapper(string sourceName, string columnName, Type dataType = null, object[] args = null);

        public abstract Guid CreateSequentialGuid();
    }
}

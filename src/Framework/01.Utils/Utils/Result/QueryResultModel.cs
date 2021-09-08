using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wyn.Utils.Result
{
    /// <summary>
    /// 查询结果模型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QueryResultModel<T>
    {
        /// <summary>
        /// 总数
        /// </summary>
        public long Total { get; set; }

        /// <summary>
        /// 数据集
        /// </summary>
        public IList<T> Rows { get; set; }

        /// <summary>
        /// 其他数据
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object Data { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="rows">数据集</param>
        /// <param name="total">总数</param>
        public QueryResultModel(IList<T> rows, long total)
        {
            Rows = rows;
            Total = total;
        }
    }
}

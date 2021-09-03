using System.Threading.Tasks;

namespace Wyn.Cache.Abstractions
{
    /// <summary>
    /// 缓存处理器
    /// </summary>
    public interface ICacheHandler
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        string Get(string key);

        /// <summary>
        /// 获取
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        T Get<T>(string key);

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        Task<string> GetAsync(string key);

        /// <summary>
        /// 获取
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        Task<T> GetAsync<T>(string key);

        /// <summary>
        /// 尝试获取
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns>真或假</returns>
        bool TryGetValue(string key, out string value);

        /// <summary>
        /// 尝试获取
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns>真或假</returns>
        bool TryGetValue<T>(string key, out T value);

        /// <summary>
        /// 设置
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        bool Set<T>(string key, T value);

        /// <summary>
        /// 设置
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expires">有效期(分钟)</param>
        bool Set<T>(string key, T value, int expires);

        /// <summary>
        /// 设置
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns>真或假</returns>
        Task<bool> SetAsync<T>(string key, T value);

        /// <summary>
        /// 设置
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expires">有效期(分钟)</param>
        /// <returns>真或假</returns>
        Task<bool> SetAsync<T>(string key, T value, int expires);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key">键</param>
        bool Remove(string key);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>真或假</returns>
        Task<bool> RemoveAsync(string key);

        /// <summary>
        /// 指定键是否存在
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>真或假</returns>
        bool Exists(string key);

        /// <summary>
        /// 指定键是否存在
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        Task<bool> ExistsAsync(string key);

        /// <summary>
        /// 删除指定前缀的缓存
        /// </summary>
        /// <param name="prefix">前缀</param>
        /// <returns></returns>
        Task RemoveByPrefixAsync(string prefix);
    }
}

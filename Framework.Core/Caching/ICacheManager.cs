using System;

namespace Framework.Core.Caching
{
    /// <summary>
    /// 缓存管理接口
    /// </summary>
    public interface ICacheManager : IDisposable
    {

        /// <summary>
        /// 获取或设置指定键值相关联的值。
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键值</param>
        /// <returns>与指定键相关联的值。</returns>
        T Get<T>(string key);

        /// <summary>
        /// 将指定的键和值添加到缓存。
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="data">数据值</param>
        /// <param name="cacheTime">缓存时间/min</param>
        void Set(string key, object data, int cacheTime);

        /// <summary>
        /// 将指定的键和值添加到缓存。
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="data">数据值</param>
        /// <param name="cacheTime">指定过期时间</param>
        void Set(string key, object data, DateTime cacheTime);

        /// <summary>
        /// 获取一个值，指与该键是否有关联的值。
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>结果</returns>
        bool Any(string key);

        /// <summary>
        /// 从缓存中删除指定键和值
        /// </summary>
        /// <param name="key">键</param>
        void Remove(string key);

        /// <summary>
        /// 清空所有缓存
        /// </summary>
        void Clear();

    }
}

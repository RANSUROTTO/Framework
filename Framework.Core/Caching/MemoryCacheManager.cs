using System;
using System.Runtime.Caching;

namespace Framework.Core.Caching
{
    /// <summary>
    /// MemoryCache缓存管理实现类
    /// </summary>
    public partial class MemoryCacheManager : ICacheManager
    {

        /// <summary>
        /// 获取MemoryCache缓存对象
        /// </summary>
        protected ObjectCache Cache => MemoryCache.Default;

        /// <summary>
        /// 获取或设置指定键值相关联的值。
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键值</param>
        /// <returns>与指定键相关联的值。</returns>
        public T Get<T>(string key)
        {
            return (T)Cache[key];
        }

        /// <summary>
        /// 将指定的键和值添加到缓存。
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="data">数据值</param>
        /// <param name="cacheTime">缓存时间/min</param>
        public void Set(string key, object data, int cacheTime)
        {
            if (data == null)
                return;

            var policy = new CacheItemPolicy { AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime) };
            Cache.Add(new CacheItem(key, data), policy);
        }

        /// <summary>
        /// 将指定的键和值添加到缓存。
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="data">数据值</param>
        /// <param name="cacheTime">指定过期时间</param>
        public void Set(string key, object data, DateTime cacheTime)
        {
            if (data == null)
                return;

            var policy = new CacheItemPolicy { AbsoluteExpiration = cacheTime };
            Cache.Add(new CacheItem(key, data), policy);
        }

        /// <summary>
        /// 获取一个值，指与该键是否有关联的值。
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>结果</returns>
        public bool Any(string key)
        {
            return (Cache.Contains(key));
        }

        /// <summary>
        /// 从缓存中删除指定键和值
        /// </summary>
        /// <param name="key">键</param>
        public void Remove(string key)
        {
            Cache.Remove(key);
        }

        /// <summary>
        /// 清空所有缓存
        /// Clear all cache data
        /// </summary>
        public void Clear()
        {
            foreach (var item in Cache)
                Remove(item.Key);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public virtual void Dispose() { }

    }
}

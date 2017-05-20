using System;
using System.Web;
using System.Collections;

namespace Framework.Core.Caching
{
    /// <summary>
    /// HTTP请求期间的缓存管理
    /// </summary>
    public partial class PerRequestCacheManager : ICacheManager
    {

        private readonly HttpContextBase _context;

        #region Ctor

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context">Context</param>
        public PerRequestCacheManager(HttpContextBase context)
        {
            this._context = context;
        }

        #endregion

        /// <summary>
        /// 创建NopRequestCache类的新实例
        /// </summary>
        /// <returns></returns>
        protected virtual IDictionary GetItems()
        {
            return _context?.Items;
        }

        /// <summary>
        /// 获取或设置指定键值相关联的值。
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键值</param>
        /// <returns>与指定键相关联的值。</returns>
        public virtual T Get<T>(string key)
        {
            var items = GetItems();
            if (items == null)
                return default(T);

            return (T)items[key];
        }

        /// <summary>
        /// 将指定的键和值添加到缓存。
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="data">数据值</param>
        /// <param name="cacheTime">缓存时间/min</param>
        public void Set(string key, object data, int cacheTime = -1)
        {
            var items = GetItems();
            if (items == null)
                return;

            if (data != null)
            {
                if (items.Contains(key))
                    items[key] = data;
                else
                    items.Add(key, data);
            }
        }

        /// <summary>
        /// 将指定的键和值添加到缓存。
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="data">数据值</param>
        /// <param name="cacheTime">指定过期时间</param>
        public void Set(string key, object data, DateTime cacheTime)
        {
            this.Set(key, data);
        }

        /// <summary>
        /// 获取一个值，指与该键是否有关联的值。
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>结果</returns>
        public bool Any(string key)
        {
            var items = GetItems();
            if (items == null)
                return false;

            return (items[key] != null);
        }

        /// <summary>
        /// 从缓存中删除指定键和值
        /// </summary>
        /// <param name="key">键</param>
        public void Remove(string key)
        {
            var items = GetItems();

            items?.Remove(key);
        }

        /// <summary>
        /// 清空所有缓存
        /// </summary>
        public void Clear()
        {
            var items = GetItems();

            items?.Clear();
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public virtual void Dispose() { }

    }
}

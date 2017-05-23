using System;

namespace Framework.Core.Data.Providers
{
    /// <summary>
    /// 数据库提供者管理
    /// </summary>
    public abstract class BaseDataProviderManager
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="settings">数据设置</param>
        protected BaseDataProviderManager(DataSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");
            this.Settings = settings;
        }

        /// <summary>
        /// 获取或设置 数据设置
        /// </summary>
        protected DataSettings Settings { get; private set; }

        /// <summary>
        /// 加载数据提供者
        /// </summary>
        public abstract IDataProvider LoadDataProvider();

    }
}

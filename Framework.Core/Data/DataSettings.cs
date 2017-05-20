using System.Collections.Generic;

namespace Framework.Core.Data
{
    /// <summary>
    /// 数据设置（连接字符串信息）
    /// </summary>
    public class DataSettings
    {

        /// <summary>
        /// 构造函数
        /// Ctor
        /// </summary>
        public DataSettings()
        {
            RawDataSettings = new Dictionary<string, string>();
        }

        /// <summary>
        /// 数据库提供者
        /// </summary>
        public string DataProvider { get; set; }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string DataConnectionString { get; set; }

        /// <summary>
        /// 设置文件
        /// </summary>
        public IDictionary<string, string> RawDataSettings { get; private set; }

        /// <summary>
        /// 指示输入的信息是否有效的值
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(this.DataProvider) && !string.IsNullOrEmpty(this.DataConnectionString);
        }



    }
}


namespace Framework.Core.Data
{
    /// <summary>
    /// 数据设置帮助
    /// </summary>
    public class DataSettingsHelper
    {

        /// <summary>
        /// 标识数据库是否已安装
        /// </summary>
        private static bool? _databaseIsInstalled;

        /// <summary>
        /// 返回一个值，表示数据库是否已经安装
        /// </summary>
        /// <returns></returns>
        public static bool DatabaseIsInstalled()
        {
            if (!_databaseIsInstalled.HasValue)
            {
                //如果为未安装则从数据设置工厂进行安装
                var manager = new DataSettingsManager();
                var settings = manager.LoadSettings();
                _databaseIsInstalled = !string.IsNullOrEmpty(settings?.DataConnectionString);
            }
            return _databaseIsInstalled.Value;
        }

        //标识重新读取配置文件
        public static void ResetCache()
        {
            _databaseIsInstalled = null;
        }

    }
}


namespace Framework.Core.Data
{
    /// <summary>
    /// Data settings helper
    /// </summary>
    public class DataSettingsHelper
    {

        private static bool? _databaseIsInstalled;

        /// <summary>
        /// Returns a value indicating whether database is already installed
        /// </summary>
        public static bool DatabaseIsInstalled()
        {
            if (!_databaseIsInstalled.HasValue)
            {
                var manager = new DataSettingsManager();
                var settings = manager.LoadSettings();
                _databaseIsInstalled = !string.IsNullOrEmpty(settings?.DataConnectionString);
            }
            return _databaseIsInstalled.Value;
        }

        //Reset information cached in the "DatabaseIsInstalled" method
        public static void ResetCache()
        {
            _databaseIsInstalled = null;
        }

    }
}

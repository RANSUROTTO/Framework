using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Framework.Core.Data
{
    /// <summary>
    /// Manager of data settings (connection string)
    /// </summary>
    public class DataSettingsManager
    {

        protected const char Separator = ':';

        /// <summary>
        /// Database configuration file name
        /// </summary>
        protected const string FileName = "db.Config";

        /// <summary>
        /// Parse settings
        /// </summary>
        /// <param name="text">Text of settings file</param>
        /// <returns>Parsed data settings</returns>
        protected virtual DataSettings ParseSettings(string text)
        {
            var shellSettings = new DataSettings();
            if (String.IsNullOrEmpty(text))
                return shellSettings;

            XElement root = XDocument.Parse(text).Root;
            var dbs = root?.Elements();
            //读取xml节点获得数据库连接配置
            dbs?.ToList().ForEach(t =>
            {
                string key = t.Attribute("key").Value;
                string value = t.Attribute("value").Value;

                switch (key)
                {
                    case "DataProvider":
                        shellSettings.DataProvider = value;
                        break;
                    case "DataConnectionString":
                        //获取数据库连接字符串 此处可进行解密读取
                        shellSettings.DataConnectionString = value;
                        break;
                    default:
                        shellSettings.RawDataSettings.Add(key, value);
                        break;
                }
            });
            return shellSettings;
        }

        /// <summary>
        /// Convert data settings to string representation
        /// </summary>
        /// <param name="settings">Settings</param>
        /// <returns>Text</returns>
        protected virtual XDocument ComposeSettings(DataSettings settings)
        {
            if (settings == null)
                return null;
            XDocument doc = new XDocument(
                new XElement("root",
                new XElement("item",
                new XAttribute("key", "DataProvider"),
                new XAttribute("value", settings.DataProvider)
                ),
                new XElement("item",
                new XAttribute("key", "DataConnectionString"),
                new XAttribute("value", settings.DataConnectionString)
                )
                ));
            settings.RawDataSettings.Keys.ToList().ForEach(t =>
            {
                doc.Root.Add(
                new XElement("item",
                new XAttribute("key", t),
                new XAttribute("value", settings.RawDataSettings[t])
                )
                    );
            });
            return doc;
        }

        /// <summary>
        /// Load settings
        /// </summary>
        /// <param name="filePath">
        /// File path; pass null to use default settings file path
        /// </param>
        /// <returns></returns>
        public virtual DataSettings LoadSettings(string filePath = null)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                //获得默认数据库配置文件路径
                filePath = Path.Combine(CommonHelper.MapPath("~/Resources/"), FileName);
            }
            if (File.Exists(filePath))
            {
                string text = File.ReadAllText(filePath);
                return ParseSettings(text);
            }

            return new DataSettings();
        }

        /// <summary>
        /// Save settings to a file
        /// </summary>
        /// <param name="settings"></param>
        public virtual void SaveSettings(DataSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");

            //获得配置存储物理路径
            string filePath = Path.Combine(CommonHelper.MapPath("~/Resources/"), FileName);

            //不存在则创建
            if (!File.Exists(filePath))
            {
                using (File.Create(filePath)) { }
            }
            //获取配置内容格式化字符串
            var doc = ComposeSettings(settings);
            //将格式化字符串写入物理路径
            doc.Save(filePath);
        }

    }
}


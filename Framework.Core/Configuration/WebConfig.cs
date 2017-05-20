using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Framework.Core.Configuration
{
    public class WebConfig : IConfigurationSectionHandler
    {

        public object Create(object parent, object configContext, XmlNode section)
        {
            return Create(section);
        }

        public WebConfig Create(XmlNode section)
        {
            var config = new WebConfig();

            var startupNode = section.SelectSingleNode("Startup");
            if (startupNode?.Attributes != null)
            {
                var attribute = startupNode.Attributes["IgnoreStartupTasks"];
                if (attribute != null)
                    config.IgnoreStartupTasks = Convert.ToBoolean(attribute.Value);
            }

            return config;
        }

        #region Util

        private string GetString(XmlNode node, string attrName)
        {
            return SetByXElement<string>(node, attrName, Convert.ToString);
        }

        private bool GetBool(XmlNode node, string attrName)
        {
            return SetByXElement<bool>(node, attrName, Convert.ToBoolean);
        }

        private T SetByXElement<T>(XmlNode node, string attrName, Func<string, T> converter)
        {
            if (node == null || node.Attributes == null) return default(T);
            var attr = node.Attributes[attrName];
            if (attr == null) return default(T);
            var attrVal = attr.Value;
            return converter(attrVal);
        }

        #endregion

        #region Prop

        /// <summary>
        /// 是否运行应用程序启动任务
        /// </summary>
        public bool IgnoreStartupTasks { get; set; }

        #endregion

    }
}

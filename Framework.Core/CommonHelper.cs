using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Hosting;

namespace Framework.Core
{
    /// <summary>
    /// һ�����õİ�����
    /// Represents a common helper
    /// </summary>
    public partial class CommonHelper
    {

        /// <summary>
        /// ��ȡ����·��������·��
        /// Maps a virtual path to a physical disk path.
        /// </summary>
        /// <param name="path">����·��. ���� "~/bin"</param>
        /// <returns>��������·�� "c:\inetpub\wwwroot\bin"</returns>
        public static string MapPath(string path)
        {
            if (HostingEnvironment.IsHosted)
            {
                return HostingEnvironment.MapPath(path);
            }

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            path = path.Replace("~/", "").TrimStart('/').Replace('/', '\\');
            return Path.Combine(baseDirectory, path);
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.AutoCode
{
    class Program
    {
        static void Main(string[] args)
        {
            //通过config.xml生成全局配置静态类
            //Generate a global configuration of static classes via config.xml
            SiteConfig.Build(args);
        }
    }
}

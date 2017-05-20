using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Core.Configuration;
using Framework.Core.Infrastructure.DependencyManagement;

namespace Framework.Core.Infrastructure
{
    public interface IEngine
    {

        /// <summary>
        /// 容器管理
        /// </summary>
        ContainerManager ContainerManager { get; }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="config">配置节</param>
        void Initialize(WebConfig config);

        /// <summary>
        /// 提取
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <returns></returns>
        T Resolve<T>() where T : class;

        /// <summary>
        ///  提取
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        object Resolve(Type type);

        /// <summary>
        /// 提取所有
        /// </summary>
        /// <typeparam name="T">类型的泛型</typeparam>
        /// <returns></returns>
        T[] ResolveAll<T>();

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core.Infrastructure
{
    /// <summary>
    /// 应由启动时运行的任务实现的接口
    /// </summary>
    public interface IStartupTask
    {

        /// <summary>
        /// 启动任务实现
        /// </summary>
        void Execute();

        /// <summary>
        /// 顺序
        /// </summary>
        int Order { get; }

    }
}

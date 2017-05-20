using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core.Infrastructure
{
    /// <summary>
    /// 应由启动时运行的任务实现的接口
    /// Interface which should be implemented by tasks run on startup
    /// </summary>
    public interface IStartupTask
    {

        /// <summary>
        /// 启动任务实现
        /// Executes a task
        /// </summary>
        void Execute();

        /// <summary>
        /// 顺序
        /// Order
        /// </summary>
        int Order { get; }

    }
}

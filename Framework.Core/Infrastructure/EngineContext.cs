using System.Configuration;
using System.Runtime.CompilerServices;
using Framework.Core.Configuration;

namespace Framework.Core.Infrastructure
{

    /// <summary>
    /// Provides access to the singleton instance of the engine.
    /// 提供访问引擎的单例实例。
    /// </summary>
    public class EngineContext
    {

        #region Properties

        /// <summary>
        /// Gets the singleton engine used to access services.
        /// 获取用于访问服务的单例引擎。
        /// </summary>
        public static IEngine Current
        {
            get
            {
                if (Singleton<IEngine>.Instance == null)
                {
                    Initialize();
                }
                return Singleton<IEngine>.Instance;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes a static instance of the factory.
        /// 初始化工厂的静态实例。
        /// </summary>
        /// <param name="forceRecreate">即使工厂以前已初始化，也创建一个新的工厂实例。</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IEngine Initialize(bool forceRecreate = false)
        {
            if (Singleton<IEngine>.Instance == null || forceRecreate)
            {
                Singleton<IEngine>.Instance = new FrameEngine();

                var config = ConfigurationManager.GetSection("WebConfig") as WebConfig;
                Singleton<IEngine>.Instance.Initialize(config);
            }
            return Singleton<IEngine>.Instance;
        }

        /// <summary>
        /// 将静态引擎实例设置为提供的引擎。 使用此方法提供您自己的引擎实现。
        /// Sets the static engine instance to the supplied engine. Use this method to supply your own engine implementation.
        /// </summary>
        /// <param name="engine">
        /// The engine to use.
        /// </param>
        public static void Replace(IEngine engine)
        {
            Singleton<IEngine>.Instance = engine;
        }

        #endregion

    }
}

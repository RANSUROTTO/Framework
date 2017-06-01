using System;
using System.Collections.Generic;
using System.Reflection;

namespace Framework.Core.Infrastructure.TypeFinder
{
    /// <summary>
    /// Classes implementing this interface provide information about types 
    /// to various services in the Nop engine.
    /// </summary>
    public interface ITypeFinder
    {

        /// <summary>  
        /// 获取程序集列表  
        /// </summary>  
        /// <returns>程序集列表</returns>  
        IList<Assembly> GetAssemblies();

        /// <summary>  
        /// 获取派生自assignTypeFrom类的类集合  
        /// </summary>  
        /// <param name="assignTypeFrom">指定父类</param>  
        /// <param name="onlyConcreteClasses">是否只查找具体类</param>  
        /// <returns></returns>  
        IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, bool onlyConcreteClasses = true);

        /// <summary>  
        /// 从assemblies程序集集合中获取派生自assignTypeFrom类的类集合  
        /// </summary>  
        /// <param name="assignTypeFrom">指定父类</param>  
        /// <param name="assemblies">指定被查找的程序集</param>  
        /// <param name="onlyConcreteClasses">是否只查找具体类</param>  
        /// <returns></returns>  
        IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true);

        /// <summary>  
        /// 获取派生自T类的类集合  
        /// </summary>  
        /// <typeparam name="T">指定父类</typeparam>  
        /// <param name="onlyConcreteClasses">是否只查找具体类</param>  
        /// <returns></returns>  
        IEnumerable<Type> FindClassesOfType<T>(bool onlyConcreteClasses = true);

        /// <summary>  
        /// 从assemblies程序集集合中获取派生自T类的类集合  
        /// </summary>  
        /// <typeparam name="T">指定父类</typeparam>  
        /// <param name="assemblies">指定被查找的程序集</param>  
        /// <param name="onlyConcreteClasses">是否只查找具体类</param>  
        /// <returns></returns>  
        IEnumerable<Type> FindClassesOfType<T>(IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true);

    }
}

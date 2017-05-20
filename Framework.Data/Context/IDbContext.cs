using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using Framework.Data.Domain;

namespace Framework.Data.Context
{
    public interface IDbContext
    {

        /// <summary>
        /// 获取对应实体的集合
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns>DbSet</returns>
        IDbSet<T> Set<T>() where T : BaseEntity;

        /// <summary>
        /// 保存更改
        /// </summary>
        /// <returns></returns>
        int SaveChanges();

        /// <summary>
        /// 保存更改
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// 分离实体
        /// </summary>
        /// <param name="entity">实体</param>
        void Detach(object entity);

        /// <summary>
        /// 修改数据状态
        /// </summary>
        /// <param name="t"></param>
        /// <param name="status"></param>
        void SetStatus<T>(T t, EntityState status) where T : BaseEntity;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        DbEntityEntry Entry<T>(T t) where T : BaseEntity;

        /// <summary>
        /// 创建表的脚本
        /// </summary>
        /// <returns></returns>
        string CreateDatabaseScript();

        /// <summary>
        /// 执行存储过程并在最后加载查询列表 (Ado.net加载查询 仅针对 存储过程最后一段查询)
        /// </summary>
        /// <typeparam name="TEntity">查询返回的对象的类型。</typeparam>
        /// <param name="commandText">命令文本</param>
        /// <param name="parameters">参数</param>
        /// <returns>Entities</returns>
        IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters)
            where TEntity : BaseEntity, new();

        /// <summary>
        /// 创建将返回给定泛型类型的元素的原始SQL查询。 
        /// 该类型可以是具有与查询返回的列的名称相匹配的属性的任何类型，也可以是简单的原始类型。 
        /// 该类型不一定是实体类型。 即使返回的对象的类型是实体类型，该查询的结果也不会被上下文跟踪。
        /// </summary>
        /// <typeparam name="TElement">查询返回的对象的类型。</typeparam>
        /// <param name="sql">SQL查询字符串。</param>
        /// <param name="parameters">要应用于SQL查询字符串的参数。</param>
        /// <returns>Result</returns>
        IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters);

        /// <summary>
        /// 对数据库执行给定的DDL / DML命令。(DDL/DML:数据库增删改查语句)
        /// </summary>
        /// <param name="sql">命令字符串</param>
        /// <param name="doNotEnsureTransaction">https://msdn.microsoft.com/zh-cn/library/system.data.entity.transactionalbehavior(v=vs.113).aspx</param>
        /// <param name="timeout">超时值，以秒为单位。 空值表示将使用底层提供程序的默认值</param>
        /// <param name="parameters">要应用到命令字符串的参数。</param>
        /// <returns>执行命令后数据库返回的结果。</returns>
        int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters);

    }
}

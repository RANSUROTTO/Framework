using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Framework.Data.Domain;

namespace Framework.Data.Repository
{
    /// <summary>
    /// 操作表封装
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T : BaseEntity, new()
    {

        /// <summary>
        /// 通过主键标识获得实体
        /// </summary>
        T GetById(object id);

        /// <summary>
        /// 通过主键标识获得实体(异步)
        /// </summary>
        Task<T> GetByIdAsync(object id);

        /// <summary>
        /// 插入实体
        /// </summary>
        /// <param name="entity">Entity</param>
        int Insert(T entity);

        /// <summary>
        /// 插入实体(异步)
        /// </summary>
        Task<int> InsertAsync(T entity);

        /// <summary>
        /// 插入多个实体
        /// </summary>
        int Insert(IEnumerable<T> entities);

        /// <summary>
        /// 插入多个实体(异步)
        /// </summary>
        Task<int> InsertAsync(IEnumerable<T> entities);

        /// <summary>
        /// 更新实体
        /// </summary>
        int Update(T entity);

        /// <summary>
        /// 更新实体
        /// </summary>
        Task<int> UpdateAsync(T entity);

        /// <summary>
        /// 更新多个实体
        /// </summary>
        int Update(IEnumerable<T> entities);

        /// <summary>
        /// 更新实体的指定属性
        /// </summary>
        int Update(T entity, params Expression<Func<T, object>>[] fields);

        /// <summary>
        /// 更新实体的指定属性
        /// </summary>
        Task<int> UpdateAsync(T entity, params Expression<Func<T, object>>[] fields);

        /// <summary>
        /// 删除实体
        /// </summary>
        void Delete(T entity);

        /// <summary>
        /// 删除实体
        /// </summary>
        Task DeleteAsync(T entity);

        /// <summary>
        /// 删除多个实体
        /// </summary>
        /// <param name="entities">实体集合</param>
        void Delete(IEnumerable<T> entities);

        /// <summary>
        /// 表 Table
        /// </summary>
        IQueryable<T> Table { get; }

        /// <summary>
        /// 不跟踪变更的表
        /// </summary>
        IQueryable<T> TableNoTracking { get; }

    }
}

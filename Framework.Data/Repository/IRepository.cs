using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Framework.Data.Domain;

namespace Framework.Data.Repository
{
    /// <summary>
    /// Repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T : BaseEntity, new()
    {

        /// <summary>
        /// Get entity by identifier
        /// </summary>
        T GetById(object id);

        /// <summary>
        /// Get entity by identifier async
        /// </summary>
        Task<T> GetByIdAsync(object id);

        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        int Insert(T entity);

        /// <summary>
        /// Insert entity async
        /// </summary>
        Task<int> InsertAsync(T entity);

        /// <summary>
        /// Insert entities
        /// </summary>
        int Insert(IEnumerable<T> entities);

        /// <summary>
        /// Insert entities async
        /// </summary>
        Task<int> InsertAsync(IEnumerable<T> entities);

        /// <summary>
        /// Update entity
        /// </summary>
        int Update(T entity);

        /// <summary>
        /// Update entity async
        /// </summary>
        Task<int> UpdateAsync(T entity);

        /// <summary>
        /// update entities
        /// </summary>
        int Update(IEnumerable<T> entities);

        /// <summary>
        /// Update the specified attribute of the entity
        /// </summary>
        int Update(T entity, params Expression<Func<T, object>>[] fields);

        /// <summary>
        /// Update the specified attribute of the entity async
        /// </summary>
        Task<int> UpdateAsync(T entity, params Expression<Func<T, object>>[] fields);

        /// <summary>
        /// delete entity
        /// </summary>
        void Delete(T entity);

        /// <summary>
        /// delete entity async
        /// </summary>
        Task DeleteAsync(T entity);

        /// <summary>
        /// delete entities
        /// </summary>
        /// <param name="entities">实体集合</param>
        void Delete(IEnumerable<T> entities);

        /// <summary>
        /// Gets a table
        /// </summary>
        IQueryable<T> Table { get; }

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        IQueryable<T> TableNoTracking { get; }

    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity.Validation;
using System.Threading.Tasks;
using Framework.Data.Context;
using Framework.Data.Domain;

namespace Framework.Data.Repository
{
    public class EfRepository<T> : IRepository<T> where T : BaseEntity, new()
    {

        #region Fields

        private readonly IDbContext _context;
        private IDbSet<T> _entities;

        #endregion

        #region Properties

        /// <summary>
        /// Table
        /// </summary>
        public virtual IQueryable<T> Table
        {
            get
            {
                return Entities;
            }
        }

        /// <summary>
        /// Get DbSet
        /// </summary>
        protected virtual IDbSet<T> Entities
        {
            get
            {
                if (_entities == null)
                    _entities = _context.Set<T>();
                return _entities;
            }
        }

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        public virtual IQueryable<T> TableNoTracking
        {
            get
            {
                return Entities.AsNoTracking();
            }
        }

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="context">Context</param>
        public EfRepository(IDbContext context)
        {
            this._context = context;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Get full error
        /// </summary>
        /// <param name="exc">Exception</param>
        /// <returns>Error</returns>
        protected string GetFullErrorText(DbEntityValidationException exc)
        {
            var msg = string.Empty;
            foreach (var validationErrors in exc.EntityValidationErrors)
                foreach (var error in validationErrors.ValidationErrors)
                    msg += string.Format("Property: {0} Error: {1}", error.PropertyName, error.ErrorMessage) + Environment.NewLine;
            return msg;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private string GetName(Expression<Func<T, object>> expression)
        {
            string name = string.Empty;

            if (expression.Body is UnaryExpression)
            {
                name = ((MemberExpression)((UnaryExpression)expression.Body).Operand).Member.Name;
            }
            else if (expression.Body is MemberExpression)
            {
                name = ((MemberExpression)expression.Body).Member.Name;
            }
            else if (expression.Body is ParameterExpression)
            {
                name = ((ParameterExpression)expression.Body).Type.Name;
            }
            return name;
        }

        #endregion

        #region Methods

        #region GetById

        public T GetById(object id)
        {
            return Entities.Find(id);
        }

        public async Task<T> GetByIdAsync(object id)
        {
            return await Table.FirstOrDefaultAsync(t => (object)t.Id == id);
        }

        #endregion

        #region Insert

        public int Insert(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                Entities.Add(entity);

                return _context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

        public async Task<int> InsertAsync(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");
                entity.CreateAt = DateTime.UtcNow;
                _entities.Add(entity);
                return await _context.SaveChangesAsync();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

        public int Insert(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");

                foreach (var entity in entities)
                    Entities.Add(entity);

                return _context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

        public async Task<int> InsertAsync(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");

                foreach (var entity in entities)
                    Entities.Add(entity);

                return await _context.SaveChangesAsync();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

        #endregion

        #region Update

        public int Update(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                return _context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

        public async Task<int> UpdateAsync(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                return await _context.SaveChangesAsync();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

        public int Update(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");

                return _context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

        public int Update(T entity, params Expression<Func<T, object>>[] fields)
        {
            T u = GetById(entity.Id);
            var t = entity.GetType();
            if (u != null)
            {   // 反射设置字段值然后更新数据库
                foreach (var field in fields)
                {
                    var p = t.GetProperty(GetName(field));
                    var value = p.GetValue(entity);
                    p.SetValue(u, value);
                }
                return Update(u);
            }
            return 0;
        }

        public async Task<int> UpdateAsync(T entity, params Expression<Func<T, object>>[] fields)
        {
            T u = GetById(entity.Id);
            var t = entity.GetType();
            if (u != null)
            {   // 反射设置字段值然后更新数据库
                foreach (var field in fields)
                {
                    var p = t.GetProperty(GetName(field));
                    var value = p.GetValue(entity);
                    p.SetValue(u, value);
                }
                return await UpdateAsync(u);
            }
            return 0;
        }

        #endregion

        #region Delete

        public void Delete(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                //不删除真实数据,仅更改属性'已删除'为真
                entity.IsDelete = true;
                _context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

        public async Task DeleteAsync(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");
                entity.IsDelete = true;
                await _context.SaveChangesAsync();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

        public void Delete(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");
                foreach (var entity in entities)
                    entity.IsDelete = true;
                _context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

        #endregion

        #endregion

    }
}

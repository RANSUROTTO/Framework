using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Framework.Data.Domain;

namespace Framework.Data.Context
{
    /// <summary>
    /// 数据库上下文 (非最终实现)
    /// </summary>
    public abstract class FrameDbContext : DbContext, IDbContext
    {

        #region Ctor

        public FrameDbContext(string nameOrConnectionString) : base(nameOrConnectionString) { }

        #endregion

        #region Utilities

        /// <summary>
        /// 将实体附加到上下文或返回已附加的实体（如果已经附加）
        /// </summary>
        /// <typeparam name="TEntity">派生自BaseEntity的实体模型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <returns>附加到上下文后的实体对象</returns>
        protected virtual TEntity AttachEntityToContext<TEntity>(TEntity entity) where TEntity : BaseEntity, new()
        {
            var alreadyAttached = Set<TEntity>().Local.FirstOrDefault(x => x.Id == entity.Id);
            if (alreadyAttached == null)
            {
                //附加新实体
                Set<TEntity>().Attach(entity);
                return entity;
            }
            return alreadyAttached;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get DbSet
        /// </summary>
        public new IDbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return base.Set<TEntity>();
        }

        /// <summary>
        /// 分离实体
        /// </summary>
        public void Detach(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            ((IObjectContextAdapter)this).ObjectContext.Detach(entity);
        }

        public void SetStatus<T>(T t, EntityState status) where T : BaseEntity
        {
            Entry(t).State = status;
        }

        DbEntityEntry IDbContext.Entry<T>(T t)
        {
            return Entry(t);
        }

        /// <summary>
        /// 执行存储过程并返回查询列表
        /// </summary>
        public IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters)
            where TEntity : BaseEntity, new()
        {
            //add parameters to command
            if (parameters != null && parameters.Length > 0)
            {
                for (int i = 0; i <= parameters.Length - 1; i++)
                {
                    var p = parameters[i] as DbParameter;
                    if (p == null)
                        throw new Exception("Not support parameter type");

                    commandText += i == 0 ? " " : ", ";

                    commandText += "@" + p.ParameterName;
                    if (p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Output)
                    {
                        //output parameter
                        commandText += " output";
                    }
                }
            }
            var result = this.Database.SqlQuery<TEntity>(commandText, parameters).ToList();
            return result;
        }

        /// <summary>
        /// 执行纯sql查询
        /// </summary>
        public IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters)
        {
            return this.Database.SqlQuery<TElement>(sql, parameters);
        }

        /// <summary>
        /// 对数据库执行给定的DDL / DML命令。
        /// </summary>
        public int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, int? timeout = null,
            params object[] parameters)
        {
            int? previousTimeout = null;
            if (timeout.HasValue)
            {
                //存储上一个超时
                previousTimeout = ((IObjectContextAdapter)this).ObjectContext.CommandTimeout;
                //设置超时时间
                ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = timeout;
            }
            var transactionalBehavior = doNotEnsureTransaction
              ? TransactionalBehavior.DoNotEnsureTransaction
              : TransactionalBehavior.EnsureTransaction;

            var result = this.Database.ExecuteSqlCommand(transactionalBehavior, sql, parameters);

            if (timeout.HasValue)
            {
                //设置以前的超时
                ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = previousTimeout;
            }

            return result;
        }


        public abstract string CreateDatabaseScript();

        #endregion

    }
}

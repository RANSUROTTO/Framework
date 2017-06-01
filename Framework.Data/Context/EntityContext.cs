using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using Framework.Data.Domain;

namespace Framework.Data.Context
{
    public class EntityContext : FrameDbContext, IDbContextFactory<EntityContext>
    {
        public EntityContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            //Open Lazy Loading 
            this.Configuration.LazyLoadingEnabled = true;
        }

        public EntityContext() : base("server=127.0.0.1,50204;database=db;uid=sa;pwd=sa") { }

        public EntityContext Create()
        {
            return this;
        }

        /// <summary>
        /// Generates scripts to create tables
        /// </summary>
        /// <returns></returns>
        public override string CreateDatabaseScript()
        {
            return ((IObjectContextAdapter)this).ObjectContext.CreateDatabaseScript();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => !String.IsNullOrEmpty(type.Namespace))
            .Where(type => type.BaseType != null && type.BaseType.IsGenericType &&
                type.BaseType.GetGenericTypeDefinition() == typeof(BaseEntity<>));
            foreach (var type in typesToRegister)
            {
                modelBuilder.RegisterEntityType(type);
            }
            base.OnModelCreating(modelBuilder);
        }

    }
}

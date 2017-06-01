using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using Framework.Core.Data.Initializers;
using Framework.Core.Data.Providers;
using Framework.Data.Context;
using MySql.Data.Entity;

namespace Framework.Data.Providers
{
    public class MySqlDataProvider : IDataProvider
    {

        private readonly string _connString;
        public MySqlDataProvider(string conn = null)
        {
            _connString = conn;
        }

        #region Methods

        public void InitConnectionFactory()
        {

            var connectionFactory = new MySqlConnectionFactory();
            if (!string.IsNullOrEmpty(_connString))
                connectionFactory.CreateConnection(_connString);
#pragma warning disable 0618
            Database.DefaultConnectionFactory = connectionFactory;
        }

        public void SetDatabaseInitializer()
        {
            string[] tablesToValidate = { };
            var customCommands = new List<string>();
            //use webHelper.MapPath instead of HostingEnvironment.MapPath which is not available in unit tests
            //customCommands.AddRange(ParseCommands(HostingEnvironment.MapPath("~/App_Data/Install/SqlServer.Indexes.sql"), false));
            //use webHelper.MapPath instead of HostingEnvironment.MapPath which is not available in unit tests
            //customCommands.AddRange(ParseCommands(HostingEnvironment.MapPath("~/App_Data/Install/SqlServer.StoredProcedures.sql"), false));

            var initializer = new CreateTablesIfNotExist<FrameDbContext>(tablesToValidate, customCommands.ToArray());
            Database.SetInitializer(initializer);
        }

        public void InitDatabase()
        {
            InitConnectionFactory();
            SetDatabaseInitializer();
        }

        public bool StoredProceduredSupported
        {
            get { return true; }
        }

        public bool BackupSupported
        {
            get { return true; }
        }

        public DbParameter GetParameter()
        {
            return new SqlParameter();
        }

        public int SupportedLengthOfBinaryHash()
        {
            return 0;
        }

        #endregion


    }
}

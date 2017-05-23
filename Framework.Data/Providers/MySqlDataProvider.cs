using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Core.Data.Providers;
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
            var connectionFactory = string.IsNullOrEmpty(_connString) ? new MySqlConnectionFactory() : new MySqlConnectionFactory(_connString);

            //TODO fix compilation warning (below)
#pragma warning disable 0618
            Database.DefaultConnectionFactory = connectionFactory;
        }

        public void SetDatabaseInitializer()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        #endregion


    }
}

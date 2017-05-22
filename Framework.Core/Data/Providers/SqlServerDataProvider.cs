using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core.Data.Providers
{
    public class SqlServerDataProvider : IDataProvider
    {

        private readonly string connString;
        public SqlServerDataProvider(string conn = null)
        {
            connString = conn;
        }


        #region Methods





        #endregion
        public void InitConnectionFactory()
        {
            throw new NotImplementedException();
        }

        public void SetDatabaseInitializer()
        {
            throw new NotImplementedException();
        }

        public void InitDatabase()
        {
            throw new NotImplementedException();
        }

        public bool StoredProceduredSupported
        {
            get { throw new NotImplementedException(); }
        }

        public bool BackupSupported
        {
            get { throw new NotImplementedException(); }
        }

        public DbParameter GetParameter()
        {
            throw new NotImplementedException();
        }

        public int SupportedLengthOfBinaryHash()
        {
            throw new NotImplementedException();
        }
    }
}

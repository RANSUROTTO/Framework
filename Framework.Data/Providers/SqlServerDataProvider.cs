using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using Framework.Core.Data.Initializers;
using Framework.Core.Data.Providers;
using Framework.Data.Context;

namespace Framework.Data.Providers
{
    public class SqlServerDataProvider : IDataProvider
    {

        private readonly string _connString;

        public SqlServerDataProvider(string conn = null)
        {
            _connString = conn;
        }

        #region Utilities

        protected virtual string[] ParseCommands(string filePath, bool throwExceptionIfNonExists)
        {
            if (!File.Exists(filePath))
            {
                if (throwExceptionIfNonExists)
                    throw new ArgumentException(string.Format("Specified file doesn't exist - {0}", filePath));

                return new string[0];
            }


            var statements = new List<string>();
            using (var stream = File.OpenRead(filePath))
            using (var reader = new StreamReader(stream))
            {
                string statement;
                while ((statement = ReadNextStatementFromStream(reader)) != null)
                {
                    statements.Add(statement);
                }
            }

            return statements.ToArray();
        }

        protected virtual string ReadNextStatementFromStream(StreamReader reader)
        {
            var sb = new StringBuilder();

            while (true)
            {
                var lineOfText = reader.ReadLine();
                if (lineOfText == null)
                {
                    if (sb.Length > 0)
                        return sb.ToString();

                    return null;
                }

                if (lineOfText.TrimEnd().ToUpper() == "GO")
                    break;

                sb.Append(lineOfText + Environment.NewLine);
            }

            return sb.ToString();
        }

        #endregion

        #region Methods

        /// <summary>
        /// 初始化连接工厂
        /// </summary>
        public void InitConnectionFactory()
        {
            var connectionFactory = string.IsNullOrEmpty(_connString) ? new SqlConnectionFactory() : new SqlConnectionFactory(_connString);

            //TODO fix compilation warning (below)
#pragma warning disable 0618
            Database.DefaultConnectionFactory = connectionFactory;
        }

        /// <summary>
        /// 设置数据库初始化程序
        /// </summary>
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

        /// <summary>
        /// 初始化数据库
        /// </summary>
        public void InitDatabase()
        {
            InitConnectionFactory();
            SetDatabaseInitializer();
        }

        /// <summary>
        /// 指示该数据提供者是否支持存储过程的值
        /// </summary>
        public bool StoredProceduredSupported
        {
            get { return true; }
        }

        /// <summary>
        /// 一个值，表示该数据提供者是否支持备份
        /// </summary>
        public bool BackupSupported
        {
            get { return true; }
        }

        /// <summary>
        /// 获取支持数据库参数对象（由存储过程使用）
        /// </summary>
        /// <returns></returns>
        public DbParameter GetParameter()
        {
            return new SqlParameter();
        }

        /// <summary>
        /// HASHBYTES功能的最大数据长度
        /// 如果不支持HASHBYTES函数，则返回0
        /// </summary>
        /// <returns></returns>
        public int SupportedLengthOfBinaryHash()
        {
            return 8000; //对于SQL Server 2008及以上HASHBYTES函数的限制为8000个字符。
        }

        #endregion

    }
}

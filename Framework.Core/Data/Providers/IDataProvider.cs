using System.Data.Common;

namespace Framework.Core.Data.Providers
{
    /// <summary>
    /// 数据提供接口
    /// </summary>
    public interface IDataProvider
    {

        /// <summary>
        /// 初始化连接工厂
        /// </summary>
        void InitConnectionFactory();

        /// <summary>
        /// 设置数据库初始化程序
        /// </summary>
        void SetDatabaseInitializer();

        /// <summary>
        /// 初始化数据库
        /// </summary>
        void InitDatabase();

        /// <summary>
        /// 指示该数据提供者是否支持存储过程的值
        /// </summary>
        bool StoredProceduredSupported { get; }

        /// <summary>
        /// 一个值，表示该数据提供者是否支持备份
        /// </summary>
        bool BackupSupported { get; }

        /// <summary>
        /// 获取支持数据库参数对象（由存储过程使用）
        /// </summary>
        /// <returns></returns>
        DbParameter GetParameter();

        /// <summary>
        /// HASHBYTES功能的最大数据长度如果不支持HASHBYTES函数，则返回0
        /// </summary>
        /// <returns>HASHBYTES功能的数据长度</returns>
        int SupportedLengthOfBinaryHash();

    }
}


using System;

namespace Framework.Core.Data.Providers
{
    public class EfDataProviderManager : BaseDataProviderManager
    {
        public EfDataProviderManager(DataSettings settings) : base(settings)
        {
        }

        public override IDataProvider LoadDataProvider()
        {
            //从数据库设置中读取数据库名称
            var providerName = Settings.DataProvider;
            if (String.IsNullOrWhiteSpace(providerName))
                throw new Exception("Data Settings doesn't contain a providerName");

            //根据不同的数据库创建不同的数据提供接口
            switch (providerName.ToLowerInvariant())
            {
                case "sqlserver":
                //return new SqlServerDataProvider();
                case "sqlce":
                //return new SqlCeDataProvider();
                default:
                    throw new Exception(string.Format("Not supported dataprovider name: {0}", providerName));
            }
        }

    }
}

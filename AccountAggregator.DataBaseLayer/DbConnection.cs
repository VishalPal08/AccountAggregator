using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;

namespace AccountAggregator.DataBaseLayer
{


    public sealed class DbConnection
    {
        public static string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
        public static string projectPath = appDirectory.Substring(0, appDirectory.IndexOf("\\bin"));

        public IConfiguration configuration = null;


        private static DbConnection _singleInstance = null;
        private static readonly object lockObject = new object();
        private DbConnection() { }

        public static DbConnection SingleInstance
        {
            get
            {
                lock (lockObject)
                {
                    if (_singleInstance == null)
                    {
                        _singleInstance = new DbConnection();
                    }

                }
                return _singleInstance;
            }
        }
        public IDbConnection connection
        {
            get
            {
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                configuration = new ConfigurationBuilder().SetBasePath(projectPath)
                                       .AddJsonFile(string.Format("appsettings.{0}.json", environment), optional: false, reloadOnChange: true)
                                       .Build();

                return new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
            }
        }
        

    }


    
}

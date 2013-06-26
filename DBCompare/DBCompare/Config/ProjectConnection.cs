using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;

namespace DBCompare.Config
{
    public class ProjectConnection
    {
        private ProjectConnection ConnectionA;

        public ProjectConnection(ProjectConnection other)
        {
            Provider = other.Provider;
            ConnectionString = other.ConnectionString;
        }
        public ProjectConnection()
        {

        }
        public void SetCatalog(string catalogName)
        {
            var builder = new SqlConnectionStringBuilder(ConnectionString);
            builder.InitialCatalog = catalogName;
            ConnectionString = builder.ConnectionString;
        }
        public DataProvider Provider { get; set; }
        public string ConnectionString { get; set; }
        public IDbConnection CreateConnection()
        {
            switch (Provider)
            {
                case DataProvider.SQlServer:
                    return new SqlConnection(ConnectionString);
                default:
                    throw new NotImplementedException();
            }
        }
        public override string ToString()
        {
            var builder = new SqlConnectionStringBuilder(ConnectionString);
            return string.Format("[{1}] on {0}", builder.DataSource, builder.InitialCatalog);
        }

        internal bool TryConnect()
        {
            try
            {
                using (var connection = CreateConnection())
                {
                    connection.Open();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

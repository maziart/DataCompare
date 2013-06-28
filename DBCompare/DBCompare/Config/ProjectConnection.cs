using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using System.Text.RegularExpressions;

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
        private const string DataBaseTimeRegex = @"^(.+?)-(\d{4})(\d{2})(\d{2})(\d{2})(\d{2})$";
        private const string DataBaseTimeFormat = "[$1] at $3/$4/$2 $5:$6";
        public override string ToString()
        {
            var builder = new SqlConnectionStringBuilder(ConnectionString);
            var database = Regex.Replace(builder.InitialCatalog, DataBaseTimeRegex, DataBaseTimeFormat);
            if (database == builder.InitialCatalog)
                database = string.Format("[{0}]", builder.InitialCatalog);
            return string.Format("{0} on {1}", database, builder.DataSource);
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

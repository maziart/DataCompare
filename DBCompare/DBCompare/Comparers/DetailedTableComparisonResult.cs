using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBCompare.Model;
using DBCompare.DAL;

namespace DBCompare.Comparers
{
    internal class DetailedTableComparisonResult
    {
        public string SchemaName { get { return Origin.SchemaName; } }
        public string TableName { get { return Origin.TableName; } }
        public PrimaryKey PrimaryKey { get { return Origin.GetPrimaryKey(); } }
        public bool AnyColumnIdentity
        {
            get { return Origin.GetAnyColumnIdentity(); }
        }
        TableComparisonResult Origin;
        public List<Column> Columns { get; set; }
        public string[] ColumnNames { get; private set; }
        public Dictionary<Key, Dictionary<string, object>> TableA { get; set; }
        public Dictionary<Key, Dictionary<string, object>> TableB { get; set; }

        public DetailedTableComparisonResult(TableComparisonResult result)
        {
            Origin = result;
            var connectionA = result.GetConnectionA();
            var connectionB = result.GetConnectionB();
            Columns = result.GetColumns();
            ColumnNames = Columns.Except(PrimaryKey.Columns).Select(n => n.Name).ToArray();
            var aKeys = result.Changes.Where(n => n.Result == CompareResult.NotEqual || n.Result == CompareResult.NotFoundInB).Select(n => n.Key).ToArray();
            TableA = LoadTable(connectionA, aKeys);
            var bKeys = result.Changes.Where(n => n.Result == CompareResult.NotEqual || n.Result == CompareResult.NotFoundInA).Select(n => n.Key).ToArray();
            TableB = LoadTable(connectionB, bKeys);
        }


        private Dictionary<Key, Dictionary<string, object>> LoadTable(System.Data.IDbConnection connection, Key[] keys)
        {
            var table = new Dictionary<Key, Dictionary<string, object>>();
            if (keys.Length == 0)
                return table;
            using (var reader = DataAccess.GetReaderByKeys(connection, SchemaName, TableName, keys, PrimaryKey, ColumnNames))
            {
                while (reader.Read())
                {
                    var row = new Dictionary<string, object>();
                    foreach (var column in Columns)
                    {
                        row[column.Name] = reader[column.Name];
                    }
                    table.Add(DataAccess.CreateKey(PrimaryKey, reader), row);
                }
            }
            return table;
        }
    }
}

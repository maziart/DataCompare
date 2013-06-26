using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using DBCompare.Model;
using DBCompare.DAL;
using DBCompare.Config;
using DBCompare.Actions;

namespace DBCompare.Comparers
{
    class TableComparer
    {
        private static readonly string[] InvalidCheckSumColumns = new string[] { "text", "ntext", "image", "xml", "sql_variant" };
        private IDbConnection ConnectionA;
        private IDbConnection ConnectionB;
        public string TableName { get; private set; }
        public string SchemaName { get; private set; }
        private List<Column> Columns;
        private PrimaryKey PrimaryKey;
        private string[] CheckSumColumns;
        private string[] OtherColumns;
        public TableComparer(IDbConnection connectionA, IDbConnection connectionB, Table table)
            : this(connectionA, connectionB, table.SchemaName, table.TableName)
        { }
        public TableComparer(IDbConnection connectionA, IDbConnection connectionB, string schemaName, string tableName)
        {
            ConnectionA = connectionA;
            ConnectionB = connectionB;
            SchemaName = schemaName;
            TableName = tableName;
        }
        public TableComparisonResult Compare()
        {
            OnProgressChanged(new ProgressChangedEventArgs(GetComparingTitle("Loading"), 0));
            OpenConnections();
            LoadColumns();
            LoadPrimaryKey();
            OnProgressChanged(new ProgressChangedEventArgs(GetComparingTitle("Comparing CheckSums"), 10));
            var rows = CompareCheckSums();
            OnProgressChanged(new ProgressChangedEventArgs(GetComparingTitle("Comparing Complex Fields"), 50));
            CompareComplexFields(rows);
            OnProgressChanged(new ProgressChangedEventArgs(GetComparingTitle("Finished"), 100));
            return new TableComparisonResult(this, SchemaName, TableName, rows);
        }

        private string GetComparingTitle(string caption)
        {
            return string.Format("Table([{1}].[{2}]): {0}", caption, SchemaName, TableName);
        }

        private void CompareComplexFields(List<RowComparisonResult> rows)
        {
            if (GetOtherColumns().Length == 0)
                return;
            var keys = rows.Where(n => n.Result == CompareResult.Equal).Select(n => n.Key).ToArray();
            if (keys.Length == 0)
                return;
            using (var readerA = GetCompareComplexFieldsReader(ConnectionA, keys))
            using (var readerB = GetCompareComplexFieldsReader(ConnectionB, keys))
            {
                while (readerA.Read() && readerB.Read())
                {
                    CompareReaders(readerA, readerB, GetOtherColumns(), rows);
                }
            }
        }

        private IDataReader GetCompareComplexFieldsReader(IDbConnection connection, Key[] keys)
        {
            return DataAccess.GetReaderByKeys(connection, SchemaName, TableName, keys, PrimaryKey, GetOtherColumns());
        }

        private void CompareReaders(IDataReader readerA, IDataReader readerB, IEnumerable<string> columns, List<RowComparisonResult> rows)
        {
            foreach (var column in columns)
            {
                if (!CompareReaders(readerA, readerB, column))
                {
                    var key = DataAccess.CreateKey(PrimaryKey, readerA);
                    var row = rows.First(n => n.Key.Equals(key));
                    row.Result = CompareResult.NotEqual;
                }
            }
        }

        private bool CompareReaders(IDataReader readerA, IDataReader readerB, string column)
        {
            var cellA = readerA[column];
            var cellB = readerB[column];
            if (cellA == DBNull.Value)
                return cellB == DBNull.Value;
            return CompareComplexFields(cellA, cellB);
        }
        public static bool ObjectsEqual(object objectA, object objectB)
        {
            if (objectA == DBNull.Value)
                return objectB == DBNull.Value;
            return CompareComplexFields(objectA, objectB);
        }
        private static bool CompareComplexFields(object cellA, object cellB)
        {
            var type = cellA.GetType();
            if (type == typeof(byte[]))
                return CompareBytes((byte[])cellA, (byte[])cellB);
            return cellA.Equals(cellB);
        }

        private static bool CompareBytes(byte[] bytesA, byte[] bytesB)
        {
            if (bytesA.Length != bytesB.Length)
                return false;
            for (int i = 0; i < bytesA.Length; i++)
            {
                if (bytesA[i] != bytesB[i])
                    return false;
            }
            return true;
        }

        private List<RowComparisonResult> CompareCheckSums()
        {
            var listA = GetCheckSums(ConnectionA);
            var listB = GetCheckSums(ConnectionB);
            var list = new List<RowComparisonResult>();
            int a = 0, b = 0;

            while (a < listA.Count && b < listB.Count)
            {
                var compare = listA[a].Key.CompareTo(listB[b].Key);
                if (compare == 0)
                {
                    if (listA[a].CheckSum != listB[b].CheckSum)
                        list.Add(new RowComparisonResult { Key = listA[a].Key, Result = CompareResult.NotEqual });
                    else
                        list.Add(new RowComparisonResult { Key = listA[a].Key, Result = CompareResult.Equal });
                    a++;
                    b++;
                }
                else if (compare < 0)
                {
                    list.Add(new RowComparisonResult { Key = listA[a].Key, Result = CompareResult.NotFoundInB });
                    a++;
                }
                else
                {
                    list.Add(new RowComparisonResult { Key = listB[b].Key, Result = CompareResult.NotFoundInA });
                    b++;
                }
            }
            while (a < listA.Count)
                list.Add(new RowComparisonResult { Key = listA[a++].Key, Result = CompareResult.NotFoundInB });
            while (b < listB.Count)
                list.Add(new RowComparisonResult { Key = listB[b++].Key, Result = CompareResult.NotFoundInA });
            return list;
        }

        private List<KeyAndCheckSum> GetCheckSums(IDbConnection connection)
        {
            return DataAccess.GetCheckSums(connection, SchemaName, TableName, PrimaryKey, GetCheckSumColumns());
        }

        private string[] GetCheckSumColumns()
        {
            if (CheckSumColumns == null)
            {
                CheckSumColumns = Columns.Where(n => !InvalidCheckSumColumns.Contains(n.DataType.ToLower())).Select(n => n.Name).ToArray();
            }
            return CheckSumColumns;
        }
        private string[] GetOtherColumns()
        {
            if (OtherColumns == null)
            {
                OtherColumns = Columns.Where(n => InvalidCheckSumColumns.Contains(n.DataType.ToLower())).Select(n => n.Name).ToArray();
            }
            return OtherColumns;
        }

        private void LoadPrimaryKey()
        {
            if (PrimaryKey != null)
                return;
            PrimaryKey = DataAccess.GetPrimaryKey(ConnectionA, SchemaName, TableName);
            if (PrimaryKey == null || PrimaryKey.Columns.Length == 0)
                throw new InvalidOperationException("Table has no primary key: " + SchemaName + "." + TableName);
        }

        private void LoadColumns()
        {
            if (Columns != null)
                return;
            Columns = DataAccess.GetColumns(ConnectionA, SchemaName, TableName);
        }

        private void OpenConnections()
        {
            if (ConnectionA.State != ConnectionState.Open)
                ConnectionA.Open();
            if (ConnectionB.State != ConnectionState.Open)
                ConnectionB.Open();
        }


        public event ProgressChangedEventHandler ProgressChanged;
        private void OnProgressChanged(ProgressChangedEventArgs e)
        {
            if (ProgressChanged != null)
                ProgressChanged(this, e);
        }

        public IDbConnection GetConnectionA()
        {
            return ConnectionA;
        }
        public IDbConnection GetConnectionB()
        {
            return ConnectionB;
        }

        public List<Column> GetColumns()
        {
            return Columns;
        }

        internal PrimaryKey GetPrimaryKey()
        {
            return PrimaryKey;
        }
        bool? AnyColumnIdentity;
        internal bool GetAnyColumnIdentity()
        {
            if (AnyColumnIdentity == null)
            {
                foreach (var column in Columns)
                {
                    AnyColumnIdentity = DataAccess.GetIsIdentity(ConnectionA, SchemaName, TableName, column.Name);
                    if (AnyColumnIdentity == true)
                        return true;
                }
            }
            return AnyColumnIdentity.Value;
        }
    }
}

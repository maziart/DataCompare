using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DBCompare.Model;
using DBCompare.DAL;

namespace DBCompare.Comparers
{
    class TableComparisonResult
    {
        private TableComparer Parent;
        public TableComparisonResult(TableComparer parent, string schemaName, string tableName, List<RowComparisonResult> rows)
        {
            Changes = rows.Where(n => n.Result != CompareResult.Equal).ToList();
            TotalComparedRows = rows.Count;
            EqualRowsCount = rows.Count - Changes.Count;
            SchemaName = schemaName;
            TableName = tableName;
            Parent = parent;
        }
        public List<RowComparisonResult> Changes { get; private set; }
        public int TotalComparedRows { get; private set; }
        public int EqualRowsCount { get; private set; }

        public string SchemaName { get; set; }
        public string TableName { get; set; }

        public override string ToString()
        {
            var builder = new StringBuilder().Append('[').Append(SchemaName).Append("].").Append('[').Append(TableName).Append("]: ");
            if (Changes.Count == 0)
                builder.Append("<Identical>");
            else if (Changes.Count == 1)
                builder.Append(Changes.Count).Append(" Change");
            else 
                builder.Append(Changes.Count).Append(" Changes");
            return builder.ToString();
        }
        internal DetailedTableComparisonResult GetDetails()
        {
            return new DetailedTableComparisonResult(this);
            
        }

        internal IDbConnection GetConnectionA()
        {
            return Parent.GetConnectionA();
        }

        internal IDbConnection GetConnectionB()
        {
            return Parent.GetConnectionB();
        }

        internal PrimaryKey GetPrimaryKey()
        {
            return Parent.GetPrimaryKey();
        }

        internal List<Column> GetColumns()
        {
            return Parent.GetColumns();
        }

        internal bool GetAnyColumnIdentity()
        {
            return Parent.GetAnyColumnIdentity();
        }
    }
}

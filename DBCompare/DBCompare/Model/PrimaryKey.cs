using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBCompare.DAL;

namespace DBCompare.Model
{
    public class PrimaryKey
    {
        public Column[] Columns { get; private set; }
        public PrimaryKey(IEnumerable<Column> columns)
        {
            Columns = columns.ToArray();
        }
        public string GetFormattedValue(Key key, string format, string seperator)
        { 
            return string.Join(seperator, Columns.Select((n,i)=>string.Format(format, n.Name,  DataAccess.GetDatabaseValue(key.Values[i], n.DataType))));
        }
        public override string ToString()
        {
            return string.Join(", ", Columns.Select(n=>n.ToString()));
        }
    }
}

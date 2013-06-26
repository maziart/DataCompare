using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBCompare.Model
{
    class TableNameAndSchema : IEquatable<TableNameAndSchema>, IComparable<TableNameAndSchema>
    {
        public string TableName { get; set; }
        public string SchemaName { get; set; }

        public bool Equals(TableNameAndSchema other)
        {
            return other != null && SchemaName == other.SchemaName && TableName == other.TableName;
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as TableNameAndSchema);
        }
        public override int GetHashCode()
        {
            return SchemaName.GetHashCode() ^ TableName.GetHashCode();
        }

        public int CompareTo(TableNameAndSchema other)
        {
            var result = this.TableName.CompareTo(other.TableName);
            if (result != 0)
                return result;
            return this.SchemaName.CompareTo(other.SchemaName);
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", TableName, SchemaName);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBCompare.Model
{
    public class Column : IEquatable<Column>
    {
        public string Name { get; set; }
        public string DataType { get; set; }
        public override string ToString()
        {
            return DataType + " " + Name;
        }

        public bool Equals(Column other)
        {
            return other != null && Name.Equals(other.Name, StringComparison.InvariantCultureIgnoreCase) && DataType.Equals(other.DataType, StringComparison.InvariantCultureIgnoreCase);
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as Column);
        }
        public override int GetHashCode()
        {
            return Name.GetHashCode() ^ DataType.GetHashCode();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBCompare.DAL;
using System.Xml.Linq;

namespace DBCompare.Model
{
    public class Key : IComparable, IEquatable<Key>
    {
        private PrimaryKey PrimaryKey;
        public Key(PrimaryKey primaryKey, IEnumerable<object> values)
        {
            PrimaryKey = primaryKey; 
            Values = values.ToArray();
        }
        public object[] Values { get; private set; }
        
        public XElement ToXml()
        {
            var element = new XElement("Key");
            for (int i = 0; i < PrimaryKey.Columns.Length; i++)
            {
                element.Add(new XElement(PrimaryKey.Columns[i].Name, Values[i]));
            }
            return element;
        }

        public int CompareTo(object obj)
        {
            Key other;
            if (!(obj is Key) || (other = (Key)obj).PrimaryKey != PrimaryKey)
                throw new NotSupportedException();
            for (int i = 0; i < PrimaryKey.Columns.Length; i++)
            {
                var compare = Comparer(i, other);
                if (compare != 0)
                    return compare;
            }
            return 0;
        }

        private int Comparer(int i, Key other)
        {
            if (PrimaryKey.Columns[i].DataType.Equals("uniqueidentifier", StringComparison.InvariantCultureIgnoreCase))
                return CompareGuid((Guid)Values[i], (Guid)other.Values[i]);
            return ((IComparable)Values[i]).CompareTo(other.Values[i]);
        }
        private static int CompareGuid(Guid a, Guid b)
        {
            var aBytes = a.ToByteArray();
            var bBytes = b.ToByteArray();
            for (int i = aBytes.Length - 1; i >= 0; i--)
            {
                var compare = aBytes[i].CompareTo(bBytes[i]);
                if (compare != 0)
                    return compare;
            }
            return 0;
        }
        public override string ToString()
        {
            return string.Join(", ", Values);
        }

        public bool Equals(Key other)
        {
            return other != null && CompareTo(other) == 0;
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as Key);
        }
        public override int GetHashCode()
        {
            var result = 0x1a2b3c4d;
            foreach (var value in Values)
            {
                if (value == null || value == DBNull.Value)
                    result ^= 0x9a9877;
                else
                    result ^= value.GetHashCode();
            }
            return result;
        }
    }
}

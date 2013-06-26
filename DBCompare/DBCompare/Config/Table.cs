using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DBCompare.Config
{
    public class Table
    {
        public Table()
        {

        }
        public Table(string fullTableName)
            : this(GetSchemaFromFullName(fullTableName), GetTableFromFullName(fullTableName))
        {
        }
        public Table(string schemaName, string tableName)
        {
            SchemaName = schemaName;
            TableName = tableName;
        }

        private static string GetTableFromFullName(string fullName)
        {
            var dotPos = fullName.IndexOf('.');
            return dotPos < 0 ? fullName : fullName.Substring(dotPos + 1).Trim('[', ']');
        }
        private static string GetSchemaFromFullName(string fullName)
        {
            var dotPos = fullName.IndexOf('.');
            return dotPos < 0 ? "dbo" : fullName.Substring(0, dotPos).Trim('[', ']');
        }
        [XmlAttribute("Schema")]
        public string SchemaName { get; set; }
        [XmlAttribute("Name")]
        public string TableName { get; set; }
    }
}

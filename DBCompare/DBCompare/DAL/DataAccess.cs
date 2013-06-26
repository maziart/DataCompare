using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBCompare.Model;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;


namespace DBCompare.DAL
{
    class DataAccess
    {
        private const string GetColumnsQuery = @"SELECT COLUMN_NAME as Name, DATA_TYPE as DataType FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = N'{0}' AND TABLE_SCHEMA = N'{1}' ORDER BY COLUMN_NAME";
        private const string GetPrimaryKeyQuery = @"SELECT c.COLUMN_NAME as Name, c.DATA_TYPE as DataType FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS a
JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE b ON a.CONSTRAINT_NAME = b.CONSTRAINT_NAME
JOIN INFORMATION_SCHEMA.COLUMNS c ON b.COLUMN_NAME = c.COLUMN_NAME AND a.TABLE_SCHEMA = c.TABLE_SCHEMA AND a.TABLE_NAME = c.TABLE_NAME
WHERE a.TABLE_NAME = N'{0}' AND a.TABLE_SCHEMA = N'{1}'AND CONSTRAINT_TYPE = 'PRIMARY KEY'";
        private const string GetCheckSumsQuery = @"SELECT {0}, CHECKSUM({1}) as [CheckSum] FROM [{2}].[{3}] ORDER BY {0}";
        private const string GetReaderByKeysQuery = @"DECLARE @XML xml
SET @XML = N'{0}'
SELECT {1}, {2} FROM [{3}].[{4}] a
JOIN (
SELECT 
{5}
FROM @XML.nodes('Keys/Key') a(c)
) b ON {6}
ORDER BY {1}";
        private const string GetTablesQuery = @"SELECT TABLE_SCHEMA as SchemaName, TABLE_NAME as TableName FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";
        private const string GetIsIdentityQuery = @"SELECT CAST(COLUMNPROPERTY(OBJECT_ID('[{0}].[{1}]'), '{2}', 'IsIdentity') AS BIT)";
        public static List<Column> GetColumns(IDbConnection connection, string schemaName, string tableName)
        {
            var command = connection.CreateCommand();
            command.CommandText = string.Format(GetColumnsQuery, tableName, schemaName);
            using (var reader = command.ExecuteReader())
            {
                return reader.ToList<Column>();
            }
        }
        public static PrimaryKey GetPrimaryKey(IDbConnection connection, string schemaName, string tableName)
        {
            var command = connection.CreateCommand();
            command.CommandText = string.Format(GetPrimaryKeyQuery, tableName, schemaName);
            using (var reader = command.ExecuteReader())
            {
                var columns = reader.ToList<Column>();
                return new PrimaryKey(columns);
            }
        }

        public static List<KeyAndCheckSum> GetCheckSums(IDbConnection connection, string schemaName, string tableName, PrimaryKey primaryKey, string[] columnNames)
        {
            var command = connection.CreateCommand();
            command.CommandText = string.Format(GetCheckSumsQuery
                , string.Join(", ", primaryKey.Columns.Select(n => string.Format("[{0}]", n.Name)))
                , string.Join(", ", columnNames.Select(n => string.Format("[{0}]", n)))
                , schemaName
                , tableName
                );
            using (var reader = command.ExecuteReader())
            {
                var result = new List<KeyAndCheckSum>();
                while (reader.Read())
                {
                    var item = new KeyAndCheckSum
                    {
                        Key = CreateKey(primaryKey, reader),
                        CheckSum = (int)reader["CheckSum"]
                    };
                    result.Add(item);
                }
                return result;
            }
        }

        public static Key CreateKey(PrimaryKey pk, IDataReader reader)
        {
            var values = new object[pk.Columns.Length];
            for (int i = 0; i < pk.Columns.Length; i++)
            {
                values[i] = reader[pk.Columns[i].Name];
            }
            return new Key(pk, values);
        }

        public static IDataReader GetReaderByKeys(IDbConnection connection, string schemaName, string tableName, Key[] keys, PrimaryKey primaryKey, string[] columnNames)
        {
            var command = connection.CreateCommand();
            command.CommandText = string.Format(GetReaderByKeysQuery
                , GetKeysXml(keys, primaryKey)
                , string.Join(", ", primaryKey.Columns.Select(n=>string.Format("a.[{0}]", n.Name)))
                , string.Join(", ", columnNames.Select(n => string.Format("[{0}]", n)))
                , schemaName
                , tableName
                , string.Join(",\n", primaryKey.Columns.Select(n => string.Format("[{0}] = c.value('(./{0})[1]','{1}{2}')", n.Name, n.DataType, n.DataType.EndsWith("char", StringComparison.InvariantCultureIgnoreCase) ? "(max)" : "")))
                , string.Join(" AND ", primaryKey.Columns.Select(n => string.Format("a.[{0}] = b.[{0}]", n.Name)))
                );
            return command.ExecuteReader();
        }
        private static string GetKeysXml(Key[] keys, PrimaryKey primaryKey)
        {
            var element = new XElement("Keys");
            foreach (var key in keys)
            {
                element.Add(key.ToXml());
            }
            return element.ToString();
        }

        public static List<TableNameAndSchema> GetTables(IDbConnection connection)
        {
            var command = connection.CreateCommand();
            command.CommandText = GetTablesQuery;
            using (var reader = command.ExecuteReader())
            {
                return reader.ToList<TableNameAndSchema>();
            }
        }

        internal static bool GetIsIdentity(IDbConnection connection, string schemaName, string tableName, string columnName)
        {
            var command = connection.CreateCommand();
            command.CommandText = string.Format(GetIsIdentityQuery, schemaName, tableName, columnName);
            var result = command.ExecuteScalar();
            if (result == DBNull.Value)
                throw new InvalidOperationException("Column not found: " + columnName);
            return (bool)result;
        }

        public static string GetDatabaseValue(object value, string dataType)
        {
            return GetDatabaseValue(value, (SqlDbType)Enum.Parse(typeof(SqlDbType), dataType == "sql_variant" ? "variant" : dataType, true), false);
        }
        private const string CastFormat = "CAST ({0} AS {1})";
        private const string CastFormatChar = "CAST ('{0}' AS {1})";
        private const string CastFormatNChar = "CAST (N'{0}' AS {1})";
        private const string FormatChar = "'{0}'";
        private const string FormatNChar = "N'{0}'";
        private const string FormatNeutral = "{0}";

        private static string GetDatabaseValue(object value, SqlDbType sqlDbType, bool includeCast)
        {
            if (value == null || value == DBNull.Value)
                return "NULL";
            switch (sqlDbType)
            {
                case SqlDbType.BigInt:
                case SqlDbType.Int:
                case SqlDbType.Float:
                case SqlDbType.Money:
                case SqlDbType.Decimal:
                case SqlDbType.TinyInt:
                case SqlDbType.Real:
                case SqlDbType.SmallInt:
                case SqlDbType.SmallMoney:
                    return string.Format(includeCast ? CastFormat : FormatNeutral , value, sqlDbType);
                case SqlDbType.Binary:
                case SqlDbType.Image:
                case SqlDbType.VarBinary:
                case SqlDbType.Timestamp:
                    var bytes = (byte[])value;
                    if (bytes.Length == 0)
                        return "0x00";
                    var builder = new StringBuilder("0x");
                    foreach (var b in bytes)
                        builder.AppendFormat("{0:X}", b);
                    return string.Format(includeCast ? CastFormat : FormatNeutral, builder, sqlDbType);
                case SqlDbType.Bit:
                    return string.Format(includeCast ? CastFormat : FormatNeutral , (bool)value ? "1" : "0", sqlDbType);
                case SqlDbType.Time:
                    return string.Format(includeCast ? CastFormatChar: FormatChar, ((TimeSpan)value).ToString("G"), sqlDbType);
                case SqlDbType.Date:
                    return string.Format(includeCast ? CastFormatChar : FormatChar, ((DateTime)value).ToString("yyyy:MM:dd"), sqlDbType);
                case SqlDbType.SmallDateTime:
                    return string.Format(includeCast ? CastFormatChar : FormatChar, ((DateTime)value).ToString("yyyy:MM:dd HH:mm:ss"), sqlDbType);
                case SqlDbType.DateTime:
                    return string.Format(includeCast ? CastFormatChar : FormatChar, ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss.fff"), sqlDbType);
                case SqlDbType.DateTime2:
                    return string.Format(includeCast ? CastFormatChar : FormatChar, ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss.fffffff"), sqlDbType);
                case SqlDbType.DateTimeOffset:
                    return string.Format(includeCast ? CastFormatChar : FormatChar, ((DateTimeOffset)value).ToString("yyyy-MM-dd HH:mm:ss.fffffff %K"), sqlDbType);
                case SqlDbType.NChar:
                case SqlDbType.NText:
                case SqlDbType.NVarChar:
                case SqlDbType.Xml:
                    return string.Format(includeCast ? CastFormatNChar: FormatNChar, value, includeCast ? GetNCharSqlDbTypeName(value, sqlDbType) : null);
                case SqlDbType.Text:
                case SqlDbType.Char:
                case SqlDbType.UniqueIdentifier:
                case SqlDbType.VarChar:
                    return string.Format(includeCast ? CastFormatChar : FormatChar, value, includeCast ? GetCharSqlDbTypeName(value, sqlDbType) : null);
                case SqlDbType.Variant:
                case SqlDbType.Udt:
                    return GetDatabaseValue(value, new SqlParameter("", value).SqlDbType, sqlDbType == SqlDbType.Variant);
                default:
                    throw new ArgumentOutOfRangeException("sqlDbType");
            }
        }

        private static string GetCharSqlDbTypeName(object value, SqlDbType sqlDbType)
        {
            switch (sqlDbType)
            {
                case SqlDbType.UniqueIdentifier:
                case SqlDbType.Text:
                    return sqlDbType.ToString();
                case SqlDbType.Char:
                case SqlDbType.VarChar:
                    return string.Format("{0}({1})", sqlDbType, ((string)value).Length);
                default:
                    throw new ArgumentOutOfRangeException("sqlDbType");
            }
        }
        private static string GetNCharSqlDbTypeName(object value, SqlDbType sqlDbType)
        {
            switch (sqlDbType)
            {
                case SqlDbType.Xml:
                case SqlDbType.NText:
                    return sqlDbType.ToString();
                case SqlDbType.NChar:
                case SqlDbType.NVarChar:
                    return string.Format("{0}({1})", sqlDbType, ((string)value).Length);
                default:
                    throw new ArgumentOutOfRangeException("sqlDbType");
            }
        }
    }
}

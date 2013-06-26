using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBCompare.Scripts;
using DBCompare.Comparers;
using DBCompare.UI;
using DBCompare.DAL;

namespace DBCompare.Service
{
    internal class ServiceAccess
    {
        public static MainForm MainForm { get; set; }
        internal static TableComparisonResult GetCompareResult(string schemaName, string tableName)
        {
            if (MainForm.CurrentComparer == null)
                return null;
            return MainForm.CurrentComparer.GetCompareResult(schemaName, tableName);
        }
        internal static List<Script> GetScripts(string schemaName, string tableName)
        {
            var list = new List<Script>();
            if (MainForm.CurrentComparer != null)
            {
                if (schemaName != null)
                {
                    var table = GetCompareResult(schemaName, tableName);
                    if (table != null)
                        list.AddRange(GenerateScript(table));
                }
                else
                {
                    var results = MainForm.CurrentComparer.GetAllResults();
                    if (results != null)
                    {
                        list.AddRange(from result in results
                                      from script in GenerateScript(result)
                                      select script);
                    }
                }
            }
            return list;
        }

        private static List<Script> GenerateScript(TableComparisonResult result)
        {
            var scripts = new List<Script>();
            var detail = result.GetDetails();
            var first = true;
            var identityInsert = false;
            foreach (var change in result.Changes)
            {
                var script = new Script();
                scripts.Add(script);
                if (first)
                {
                    script.AddToken(TokenType.Comment, string.Format("---[{0}].[{1}]---\r\n", result.SchemaName, result.TableName));
                    if (detail.AnyColumnIdentity && result.Changes.Where(n => n.Result == CompareResult.NotFoundInB).Any())
                    {
                        identityInsert = true;
                        script.AddScript(string.Format("SET IDENTITY_INSERT [{0}].[{1}] ON\r\n", result.SchemaName, result.TableName));
                        script = new Script();
                        scripts.Add(script);
                    }
                    first = false;
                }
                FillChangeScript(change, detail, script);
            }
            if (identityInsert)
            {
                scripts.Add(new Script(string.Format("SET IDENTITY_INSERT [{0}].[{1}] OFF\r\n", result.SchemaName, result.TableName)));
            }
            return scripts;
        }

        private static void FillChangeScript(RowComparisonResult change, DetailedTableComparisonResult detail, Script script)
        {
            switch (change.Result)
            {
                case CompareResult.NotFoundInA:
                    FillDelete(change, detail, script);
                    break;
                case CompareResult.NotFoundInB:
                    FillInsert(change, detail, script);
                    break;
                case CompareResult.NotEqual:
                    FillUpdate(change, detail, script);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            script.AddToken("\r\n\r\n");
        }

        private static void FillUpdate(RowComparisonResult change, DetailedTableComparisonResult result, Script script)
        {
            var rowA = result.TableA[change.Key];
            var rowB = result.TableB[change.Key];
            script.AddScript(string.Format("UPDATE [{0}].[{1}] SET\r\n", result.SchemaName, result.TableName));
            var comma = "";
            foreach (var column in result.Columns.Except(result.PrimaryKey.Columns))
            {
                var columnName = column.Name;
                var aValue = rowA[columnName];
                var bValue = rowB[columnName];
                if (!TableComparer.ObjectsEqual(aValue, bValue))
                {
                    script.AddToken(comma);
                    script.AddScript(string.Format("[{0}] = {1}", columnName, DataAccess.GetDatabaseValue(aValue, column.DataType)));
                    comma = ",\r\n";
                }
            }
            script.AddToken("\r\n WHERE");
            script.AddScript(GetPrimaryKeyCondition(change, result));
        }

        private static string GetPrimaryKeyCondition(RowComparisonResult change, DetailedTableComparisonResult result)
        {
            return result.PrimaryKey.GetFormattedValue(change.Key, "[{0}] = {1}", " AND ");
        }

        private static void FillInsert(RowComparisonResult change, DetailedTableComparisonResult result, Script script)
        {
            script.AddScript(string.Format("INSERT INTO [{0}].[{1}] (", result.SchemaName, result.TableName));
            var comma = "";
            foreach (var column in result.Columns)
            {
                script.AddToken(comma)
                      .AddToken(TokenType.Text, string.Format("[{0}]", column.Name));
                comma = ", ";
            }
            script.AddScript(")\r\nVALUES (");
            var row = result.TableA[change.Key];
            comma = "";
            foreach (var column in result.Columns)
            {
                script.AddToken(comma)
                      .AddToken(DataAccess.GetDatabaseValue(row[column.Name], column.DataType));
                comma = ", ";
            }
            script.AddToken(TokenType.GKeyword, ")");
        }

        private static void FillDelete(RowComparisonResult change, DetailedTableComparisonResult result, Script script)
        {
            script.AddScript(string.Format("DELETE FROM [{0}].[{1}] WHERE {2}", result.SchemaName, result.TableName, GetPrimaryKeyCondition(change, result)));
        }
    }
}

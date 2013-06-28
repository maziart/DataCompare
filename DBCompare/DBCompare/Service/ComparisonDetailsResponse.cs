using System;
using System.Collections.Generic;
using DBCompare.Comparers;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using DBCompare.Model;
using System.Linq;

namespace DBCompare.Service
{
    internal class ComparisonDetailsResponse : HtmlResponse
    {
        private string SchemaName;
        private string TableName;
        public ComparisonDetailsResponse(string schemaName, string tableName)
        {
            SchemaName = schemaName;
            TableName = tableName;
        }
        protected override void Render()
        {
            var result = ServiceAccess.GetCompareResult(SchemaName, TableName);
            if (result == null)
            {
                NotFound();
                return;
            }
            RenderHead();
            RenderBody(result);
            base.Render();
        }

        private void RenderBody(TableComparisonResult result)
        {
            var body = new HtmlGenericControl("body");
            var container = new Panel { CssClass = "container" };
            body.Controls.Add(container);
            container.Controls.Add(CreateTitle());
            var divBody = new Panel { CssClass = "body" };
            divBody.Controls.Add(CreateSummary(result));
            divBody.Controls.Add(CreateDetails(result));
            container.Controls.Add(divBody);
            body.RenderControl(HWriter);
        }

        private Panel CreateDetails(TableComparisonResult result)
        {
            var panel = new Panel { CssClass = "section" };
            var title = new HtmlGenericControl("h2");
            title.Controls.AddLiteral("Details");
            panel.Controls.Add(title);
            if (result.Changes.Count > 0)
            {
                var scriptLink = new HyperLink
                {
                    CssClass = "ScriptButton",
                    Text = "Script",
                    ToolTip = "Generate scripts to make Connection B equal to Connection A",
                    NavigateUrl = "/Script/" + result.SchemaName + "/" + result.TableName
                };
                title.Controls.Add(scriptLink);
            }
            var columnsButton = new HyperLink
            {
                CssClass = "ColumnsButton",
                Text = "Columns",
                ToolTip = "Change the columns' visibility",
                NavigateUrl = "#"
            };
            columnsButton.Attributes["onclick"] = "chooseColumns();";
            title.Controls.Add(columnsButton);

            var details = new Panel { CssClass = "details" };
            details.Style[System.Web.UI.HtmlTextWriterStyle.OverflowY] = "auto";
            panel.Controls.Add(details);
            if (result.Changes.Count == 0)
            {
                var tick = new Image { ImageUrl = WebService.GetFileUrl("Tick.png"), AlternateText = "Identical" };
                var identicalLabel = new Label { CssClass = "Green Font16" };
                identicalLabel.Controls.AddLiteral("Tables are identical in two databases");
                details.Controls.Add(tick);
                details.Controls.Add(identicalLabel);
                return panel;
            }
            var detailedResult = result.GetDetails();
            var table = CreateTableWithHeader(detailedResult.Columns);
            foreach (var change in result.Changes)
            {
                var row = new TableRow { CssClass = "Row" + change.Result };
                foreach (var column in detailedResult.Columns)
                {
                    row.Cells.Add(CreateCell(change.Key, column, detailedResult.TableA, detailedResult.TableB, change.Result));
                }
                table.Rows.Add(row);
            }
            details.Controls.Add(table);
            panel.Controls.Add(details);
            return panel;
        }

        private TableCell CreateCell(Key key, Column column, Dictionary<Key, Dictionary<string, object>> aList, Dictionary<Key, Dictionary<string, object>> bList, CompareResult compareResult)
        {
            var td = new TableCell { CssClass = "_" + column.Name + " " + compareResult };

            switch (compareResult)
            {
                case CompareResult.NotFoundInA:
                    td.Controls.AddLiteral(GetValueString(bList[key][column.Name], column.DataType));
                    break;
                case CompareResult.NotFoundInB:
                    td.Controls.AddLiteral(GetValueString(aList[key][column.Name], column.DataType));
                    break;
                case CompareResult.NotEqual:
                    var aValue = aList[key][column.Name];
                    var bValue = bList[key][column.Name];
                    if (!TableComparer.ObjectsEqual(aValue, bValue))
                    {
                        var p1 = new HtmlGenericControl("p");
                        p1.Attributes["class"] = "OldValue";
                        p1.Controls.AddLiteral(GetValueString(bValue, column.DataType));
                        td.Controls.Add(p1);
                        var p2 = new HtmlGenericControl("p");
                        p2.Attributes["class"] = "NewValue";
                        p2.Controls.AddLiteral(GetValueString(aValue, column.DataType));
                        td.Controls.Add(p2);
                    }
                    else
                    {
                        td.Controls.AddLiteral(GetValueString(aValue, column.DataType));
                    }
                    break;
                default:
                    break;
            }
            return td;
        }

        private string GetValueString(object value, string dataType)
        {
            if (value == DBNull.Value)
                return "<span class='Null'>NULL</span>";
            if (value.GetType() == typeof(byte[]))
                return "&lt;byte[]&gt;";
            if (dataType.Equals("xml", StringComparison.InvariantCultureIgnoreCase))
                return "&lt;xml&gt;";
            return value.ToString();
        }

        private Table CreateTableWithHeader(List<Column> columns)
        {
            var table = new Table();
            table.Attributes["border"] = "1";
            table.CssClass = "data";
            var row = new TableRow();
            foreach (var column in columns)
            {
                row.Cells.Add(CreateHeader(column.Name));
            }
            table.Rows.Add(row);
            return table;
        }
        private TableHeaderCell CreateHeader(string caption)
        {
            var header = new TableHeaderCell { CssClass = "_" + caption };
            header.Controls.AddLiteral(caption);
            return header;
        }

        private Panel CreateSummary(TableComparisonResult result)
        {
            var panel = new Panel { CssClass = "section" };
            var title = new HtmlGenericControl("h2");
            title.Controls.AddLiteral("Summary");
            panel.Controls.Add(title);
            var details = new Panel { CssClass = "details" };
            var table = new Table();
            table.Rows.Add(CreateRowByLiterals("Total Number of <strong>Checked</strong> Records:", result.TotalComparedRows.ToString()));
            table.Rows.Add(CreateRowByLiterals("Total Number of <strong>Equal</strong> Records:", result.EqualRowsCount.ToString()));
            table.Rows.Add(CreateRowByLiterals("Total Number of <strong>Changed</strong> Records:", result.Changes.Count.ToString()));
            table.Rows[0].Cells[1].CssClass = "Bold";
            table.Rows[1].Cells[1].CssClass = "Bold Green";
            table.Rows[2].Cells[1].CssClass = result.Changes.Count > 0 ? "Bold Red" : "Bold Green";
            details.Controls.Add(table);
            panel.Controls.Add(details);
            return panel;
        }
        private TableRow CreateRowByLiterals(params string[] cells)
        {
            var row = new TableRow();
            foreach (var cellValue in cells)
            {
                var cell = new TableCell();
                cell.Controls.AddLiteral(cellValue);
                row.Cells.Add(cell);
            }
            return row;
        }

        private HtmlGenericControl CreateTitle()
        {
            var title = new HtmlGenericControl("h1");
            var label = new Label();
            label.CssClass = "HighLight";
            label.Controls.AddLiteral("{0} ({1})", TableName, SchemaName);
            title.Controls.AddLiteral("Comparison results for table ");
            title.Controls.Add(label);
            return title;
        }

        private void RenderHead()
        {
            var head = new HtmlGenericControl("head");
            head.Controls.Add(new HtmlMeta
            {
                HttpEquiv = "content-Type",
                Content = "text/html; charset=utf8"
            });
            head.Controls.Add(new HtmlMeta
            {
                HttpEquiv = "Date",
                Content = DateTime.Now.ToString()
            });
            var title = new HtmlTitle();
            title.Controls.AddLiteral("[{0}].[{1}] Comparison Details - DBCompare", SchemaName, TableName);
            head.Controls.Add(title);
            head.Controls.Add(CreateStylesheetElement("jquery-ui-1.10.3.custom.min.css"));
            head.Controls.Add(CreateStylesheetElement("DBCompare.css"));
            head.Controls.Add(CreateScriptElement("jquery-1.10.1.min.js"));
            head.Controls.Add(CreateScriptElement("jquery-ui-1.10.3.custom.min.js"));
            head.Controls.Add(CreateScriptElement("DBCompare.js"));
            head.RenderControl(HWriter);
        }

        private static HtmlLink CreateStylesheetElement(string fileName)
        {
            var link = new HtmlLink();
            link.Attributes["rel"] = "Stylesheet";
            link.Attributes["type"] = "text/css";
            link.Href = WebService.GetFileUrl(fileName);
            return link;
        }

        private static HtmlGenericControl CreateScriptElement(string fileName)
        {
            var jquery = new HtmlGenericControl("script");
            jquery.Attributes["type"] = "text/javascript";
            jquery.Attributes["src"] = WebService.GetFileUrl(fileName);
            return jquery;
        }
    }
}

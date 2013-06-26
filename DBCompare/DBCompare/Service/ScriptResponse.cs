using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.HtmlControls;
using DBCompare.Scripts;
using System.Web.UI.WebControls;

namespace DBCompare.Service
{
    internal class ScriptResponse : HtmlResponse
    {
        private string SchemaName;
        private string TableName;
        public ScriptResponse()
            : this(null, null)
        {
        }
        public ScriptResponse(string schemaName, string tableName)
        {
            SchemaName = schemaName;
            TableName = tableName;
        }
        protected override void Render()
        {
            var scripts = ServiceAccess.GetScripts(SchemaName, TableName);
            if (scripts == null || scripts.Count == 0)
            {
                NotFound();
                return;
            }
            RenderHead();
            RenderBody(scripts);
            base.Render();
        }

        private void RenderBody(List<Script> scripts)
        {
            var body = new HtmlGenericControl("body");
            foreach (var script in scripts)
            {
                body.Controls.Add(CreateScriptControl(script));
            }
            body.RenderControl(HWriter);
        }

        private HtmlGenericControl CreateScriptControl(Script script)
        {
            var pre = new HtmlGenericControl("pre");
            pre.Attributes["class"] = "sql";
            foreach (var token in script.Tokens)
            {
                var label = new Label { CssClass = token.Type };
                label.Controls.AddLiteral(token.Value.Replace("<","&lt;").Replace(">", "&gt;"));
                pre.Controls.Add(label);
            }
            return pre;
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
            if (SchemaName == null)
                title.Controls.AddLiteral("Scripts - DBCompare");
            else
                title.Controls.AddLiteral("[{0}].[{1}] Scirpt - DBCompare", SchemaName, TableName);
            head.Controls.Add(title);
            var link = new HtmlLink();
            link.Attributes["rel"] = "Stylesheet";
            link.Attributes["type"] = "text/css";
            link.Href = WebService.GetFileUrl("DBCompare.css");
            head.Controls.Add(link);
            head.RenderControl(HWriter);
        }
    }
}

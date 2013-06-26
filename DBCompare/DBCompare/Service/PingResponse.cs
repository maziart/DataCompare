using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace DBCompare.Service
{
    internal class PingResponse : HtmlResponse
    {
        private string Name;
        private Panel MainPanel;
        public PingResponse(string name)
        {
            Name = name;
            MainPanel = new Panel();
            MainPanel.Style[HtmlTextWriterStyle.FontFamily] = "Tahoma";
            MainPanel.Controls.Add(new LiteralControl("Pong to "));
            var label = new Label();
            label.Style[HtmlTextWriterStyle.FontWeight] = "Bold";
            label.Style[HtmlTextWriterStyle.Color] = "Red";
            label.Controls.AddLiteral(Name);
            MainPanel.Controls.Add(label);
        }
        protected override void Render()
        {
            MainPanel.RenderControl(HWriter);
        }
    }
}

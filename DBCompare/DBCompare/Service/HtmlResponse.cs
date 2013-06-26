using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace DBCompare.Service
{
    internal class HtmlResponse : HttpResponse
    {
        protected HtmlTextWriter HWriter;
        protected override void OnInit()
        {
            base.OnInit();
            HWriter = new HtmlTextWriter(Writer);
            Writer.WriteLine("<!DOCTYPE html>");
            Writer.WriteLine("<html>");
        }
        protected override void Render()
        {
            Writer.WriteLine("</html>");
            base.Render();
        }

        protected override string ContentType
        {
            get { return "text/html"; }
        }
    }
}

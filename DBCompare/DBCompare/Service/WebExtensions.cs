using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace DBCompare.Service
{
    internal static class WebExtensions
    {
        public static void AddLiteral(this ControlCollection collection, string literal)
        {
            collection.Add(new LiteralControl(literal));
        }
        public static void AddLiteral(this ControlCollection collection, string format, params object[] args)
        {
            collection.Add(new LiteralControl(string.Format(format, args)));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBCompare.Scripts
{
    public class Token
    {
        public string Value { get; set; }
        public string Type { get; set; }
        public override string ToString()
        {
            return string.Format("{0} ({1})", Value, Type);
        }
    }
}

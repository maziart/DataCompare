using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBCompare.Model;

namespace DBCompare.Comparers
{
    class RowComparisonResult
    {
        public Key Key { get; set; }
        public CompareResult Result { get; set; }
        public override string ToString()
        {
            return Key + ": " + Result;
        }
    }
}

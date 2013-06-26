using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBCompare.Model
{
    class KeyAndCheckSum
    {
        public Key Key { get; set; }
        public int CheckSum { get; set; }
        public override string ToString()
        {
            return Key + ": " + CheckSum;
        }
    }
}

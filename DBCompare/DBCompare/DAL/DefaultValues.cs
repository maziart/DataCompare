using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBCompare.DAL
{
    class DefaultValues
    {
        public static T GetDefault<T>()
        {
            return default(T); //TODO
        }
    }
}

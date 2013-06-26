using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBCompare.DAL
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    sealed class DataReaderIgnoreAttribute : Attribute
    {
    }
}

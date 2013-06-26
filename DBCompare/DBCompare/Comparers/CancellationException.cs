using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBCompare.Comparers
{
    [Serializable]
    public class CancellationException : Exception
    {
        public CancellationException() { }
        public CancellationException(string message) : base(message) { }
        public CancellationException(string message, Exception inner) : base(message, inner) { }
        protected CancellationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBCompare.Actions
{
    internal interface IAction
    {
        event ProgressChangedEventHandler ProgressChanged;
        void SetUserState(object state, params object[] previousResults);
        object DoWork();
        bool SupportsCancellation { get; }
        void CancelAsync();
        List<Exception> Errors { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBCompare.Actions
{
    public class ProgressChangedEventArgs : EventArgs
    {
        public string CurrentOperation { get; private set; }
        public int Percentage { get; private set; }
        public ProgressChangedEventArgs(string currentOperation, int percentage)
        {
            CurrentOperation = currentOperation;
            Percentage = percentage;
        }
    }
}

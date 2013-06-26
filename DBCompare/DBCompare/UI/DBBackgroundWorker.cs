using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace DBCompare.UI
{
    class DBBackgroundWorker : BackgroundWorker
    {
        protected override void OnDoWork(DoWorkEventArgs e)
        {
            base.OnDoWork(e);
        }
        protected override void OnProgressChanged(ProgressChangedEventArgs e)
        {
            base.OnProgressChanged(e);
        }
    }
}

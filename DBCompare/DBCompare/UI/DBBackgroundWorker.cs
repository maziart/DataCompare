using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using DBCompare.Actions;
using ProgressChangedEventArgs = System.ComponentModel.ProgressChangedEventArgs;
using System.Windows.Forms;
using DBCompare.Comparers;

namespace DBCompare.UI
{
    class DBBackgroundWorker : BackgroundWorker
    {
        private List<IAction> Actions;
        private List<object> States;
        public DBBackgroundWorker()
        {
            WorkerReportsProgress = true;
            WorkerSupportsCancellation = true;
            Actions = new List<IAction>();
            States = new List<object>();
        }

        public ProgressBar ProgressBar { get; set; }
        public Label CurrentOperationLabel { get; set; }
        public Label ProgressLabel { get; set; }
        public IAction CurrentAction { get; private set; }
        public Button CancelButton { get; set; }
        protected override void OnDoWork(DoWorkEventArgs e)
        {
            var results = new List<object>();
            try
            {
                for (int i = 0; i < Actions.Count; i++)
                {
                    if (CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }
                    var action = Actions[i];
                    try
                    {
                        action.ProgressChanged += Action_ProgressChanged;
                        action.SetUserState(States[i], results.ToArray());
                        results.Add(action.DoWork());
                        if (action.Errors != null && action.Errors.Count > 0)
                        {
                            MessageBox.Show(string.Join("\n", action.Errors.Select(n => n.Message)), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    finally
                    {
                        action.ProgressChanged -= Action_ProgressChanged;
                    }
                }
                e.Result = results.ToArray();
            }
            catch (CancellationException)
            {
                e.Cancel = true;
            }
        }

        private void Action_ProgressChanged(object sender, DBCompare.Actions.ProgressChangedEventArgs e)
        {
            if (!IsBusy)
                return;
            var index = Actions.IndexOf((IAction)sender);
            var percentage = index + e.Percentage / 100f;
            percentage *= 100;
            percentage /= Actions.Count;
            ReportProgress((int)percentage, new ProgressState { Action = (IAction)sender, Operation = e.CurrentOperation });
        }
        protected override void OnProgressChanged(ProgressChangedEventArgs e)
        {
            if (ProgressBar != null)
                ProgressBar.Value = e.ProgressPercentage;
            var state = (ProgressState)e.UserState;
            if (CurrentOperationLabel != null)
                CurrentOperationLabel.Text = state.Operation;
            if (ProgressLabel != null)
                ProgressLabel.Text = e.ProgressPercentage + "%";

            CurrentAction = (IAction)state.Action;
            CancelButton.Enabled = CurrentAction.SupportsCancellation;
            if (CurrentAction.SupportsCancellation && CancellationPending)
            {
                CurrentAction.CancelAsync();
            }
            base.OnProgressChanged(e);
        }
        private class ProgressState
        {
            public IAction Action { get; set; }
            public string Operation { get; set; }
        }

        public void AddAction(IAction action, object state)
        {
            Actions.Add(action);
            States.Add(state);
        }
        public void ClearActions()
        {
            Actions.Clear();
            States.Clear();
        }
    }
}

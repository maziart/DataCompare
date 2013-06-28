using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DBCompare.Config;
using DBCompare.Actions;

namespace DBCompare.UI
{
    public partial class BackupRestoreForm : ParentForm
    {
        DataCompareProject Project;
        public BackupRestoreForm(DataCompareProject project)
        {
            Project = project;
            InitializeComponent();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy)
            {
                backgroundWorker1.CancelAsync();
                return;
            }
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            BtnStart.Enabled = false;
            backgroundWorker1.ClearActions();
            IAction action;
            object state;
            Project.CreateBackUpAction(out action, out state);
            backgroundWorker1.AddAction(action, state);
            Project.CreateRestoreAction(out action, out state);
            backgroundWorker1.AddAction(action, state);
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                DialogResult = System.Windows.Forms.DialogResult.Cancel;
                Close();
            }
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.GetBaseException().Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                BtnCancel.Enabled = BtnStart.Enabled = true;
                return;
            }
            MessageBox.Show("The database has been duplicated, now you can work with your database to see the changes", "Monitor", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
    }
}

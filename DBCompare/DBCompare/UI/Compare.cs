﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DBCompare.Config;
using DBCompare.Comparers;
using System.Diagnostics;
using DBCompare.Service;
using DBCompare.Actions;
using ProgressChangedEventArgs = System.ComponentModel.ProgressChangedEventArgs;

namespace DBCompare.UI
{
    public partial class Compare : UserControl
    {
        private DatabaseComparer Comparer;
        private DataCompareProject Project;
        private MainForm MainForm { get { return (MainForm)ParentForm; } }
        public Compare(DataCompareProject project)
        {
            InitializeComponent();
            Project = project;
            if (project.Type == ProjectType.Compare)
                BtnProject.Image = DBCompare.Properties.Resources.Compare16;
            else
                BtnProject.Image = DBCompare.Properties.Resources.Monitor16;
        }
        public void StartComparing()
        {
            LblStatus.Text = "Comparing ...";
            LblStatus.ForeColor = Color.Black;
            BtnScripts.Enabled = BtnProject.Enabled = BtnRerun.Enabled = false;
            BtnCancel.Enabled = true;
            compareAnimation1.Start();
            Actions = new List<IAction>();
            States = new List<object>();
            if (Project.RequiresBackupRestore)
            {
                IAction action;
                object state;
                Project.CreateBackUpAction(out action, out state);
                Actions.Add(action);
                States.Add(state);
                Project.CreateRestoreAction(out action, out state);
                Actions.Add(action);
                States.Add(state);
            }
            Comparer = new DatabaseComparer(Project);
            Actions.Add(Comparer);
            States.Add(null);
            backgroundWorker1.RunWorkerAsync();
        }
        private List<IAction> Actions;
        private List<object> States;
        ProjectComparisonResult Result;
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            var results = new List<object>();
            try
            {
                for (int i = 0; i < Actions.Count; i++)
                {
                    if (backgroundWorker1.CancellationPending)
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
            if (!backgroundWorker1.IsBusy)
                return;
            var index = Actions.IndexOf((IAction)sender);
            var percentage = index + e.Percentage / 100f;
            percentage *= 100;
            percentage /= Actions.Count;
            backgroundWorker1.ReportProgress((int)percentage, new { Action = sender, Operation = e.CurrentOperation });
        }
        private IAction CurrentAction;
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            dynamic state = e.UserState;
            LblCurrentOperation.Text = state.Operation;
            LblProgress.Text = e.ProgressPercentage + "%";

            CurrentAction = (IAction)state.Action;
            BtnCancel.Enabled = CurrentAction.SupportsCancellation;
            if (CurrentAction.SupportsCancellation && backgroundWorker1.CancellationPending)
            {
                CurrentAction.CancelAsync();
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                LblStatus.Text = "Cancelled";
                LblStatus.ForeColor = Color.Brown;
                Result = null;
                compareAnimation1.SetImage(null);
            }
            else
            {
                LblStatus.Text = "Done.";
                LblProgress.Text = "100%";
                progressBar1.Value = 0;
                var result = (object[])e.Result;
                Result = (ProjectComparisonResult)result.Last();
                if (Result.SourcesAreEqual())
                {
                    compareAnimation1.SetImage(DBCompare.Properties.Resources.IdenticalDatabases);
                    LblStatus.Text += " Databases are identical.";
                    LblStatus.ForeColor = Color.Green;
                }
                else
                {
                    compareAnimation1.SetImage(DBCompare.Properties.Resources.DifferentDatabases);
                    LblStatus.Text += " Databases have differences.";
                    LblStatus.ForeColor = Color.Red;
                    BtnScripts.Enabled = true;
                }
            }
            FillResults();
            LblCurrentOperation.Text = "None";
            BtnCancel.Enabled = false;
            BtnProject.Enabled = BtnRerun.Enabled = true;
        }

        private void FillResults()
        {
            dataGridView1.SuspendLayout();
            try
            {
                dataGridView1.Rows.Clear();
                if (Result == null || Result.Tables == null)
                    return;
                foreach (var table in Result.Tables)
                {
                    if (table.Changes.Count == 0 && !RdbViewAll.Checked)
                        continue;
                    var row = dataGridView1.Rows[dataGridView1.Rows.Add()];
                    row.Cells[StatusColumn.Index].Value = table.Changes.Count == 0 ? DBCompare.Properties.Resources.Tick : DBCompare.Properties.Resources.Bullet16;
                    row.Cells[TableColumn.Index].Value = string.Format("{0} ({1})", table.TableName, table.SchemaName);
                    row.Cells[RecordsColumn.Index].Value = table.TotalComparedRows;
                    row.Cells[ChangesColumn.Index].Value = table.Changes.Count;
                    row.Cells[DetailsColumn.Index].Value = "View Details";
                    row.Tag = table;
                }
            }
            finally
            {
                dataGridView1.ResumeLayout();
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (!backgroundWorker1.IsBusy)
                return;
            backgroundWorker1.CancelAsync();
        }

        private void BtnRerun_Click(object sender, EventArgs e)
        {
            StartComparing();
        }

        private void BtnProject_Click(object sender, EventArgs e)
        {
            MainForm.ShowProject(Project);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == DetailsColumn.Index)
            {
                var row = dataGridView1.Rows[e.RowIndex];
                var table = row.Tag as TableComparisonResult;
                if (table == null)
                    return;
                ShowDetails(table);
            }
        }

        private void ShowDetails(TableComparisonResult table)
        {
            Process.Start(WebService.GetComparisonDetailsUrl(table.SchemaName, table.TableName));
        }

        public void DisposeComparer()
        {
            if (Comparer != null)
                Comparer.Dispose();
        }

        internal TableComparisonResult GetCompareResult(string schemaName, string tableName)
        {
            if (Result == null)
                return null;
            return Result.Tables.FirstOrDefault(n => n.SchemaName == schemaName && n.TableName == tableName);
        }

        internal IEnumerable<TableComparisonResult> GetAllResults()
        {
            if (Result == null)
                return null;
            return Result.Tables.AsEnumerable();
        }

        private void BtnScripts_Click(object sender, EventArgs e)
        {
            Process.Start(WebService.GetScriptsUrl());
        }

        private void RdbViewAll_CheckedChanged(object sender, EventArgs e)
        {
            FillResults();
        }
    }
}

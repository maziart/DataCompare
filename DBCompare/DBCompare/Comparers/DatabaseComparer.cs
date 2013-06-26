using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBCompare.Config;
using System.Data;
using System.IO;
using System.Xml.Serialization;
using DBCompare.Actions;

namespace DBCompare.Comparers
{
    class DatabaseComparer : IAction, IDisposable
    {
        private DataCompareProject Project;
        private IDbConnection ConnectionA;
        private IDbConnection ConnectionB;
        private List<TableComparer> Tables;
        public List<Exception> Errors { get; private set; }
        public DatabaseComparer(DataCompareProject project)
        {
            Project = project;
            ConnectionA = project.ConnectionA.CreateConnection();
            ConnectionB = project.ConnectionB.CreateConnection();
            Tables = project.Tables.Select(table => CreateTable(table)).ToList();
        }

        private TableComparer CreateTable(Table table)
        {
            var comparer = new TableComparer(ConnectionA, ConnectionB, table);
            comparer.ProgressChanged += comparer_ProgressChanged;
            return comparer;
        }

        private void comparer_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var comparer = (TableComparer)sender;
            var index = Tables.IndexOf(comparer);
            var percentage = index + e.Percentage / 100f;
            percentage *= 100;
            percentage /= Tables.Count;
            OnProgressChanged(new ProgressChangedEventArgs(e.CurrentOperation, (int)percentage));
        }
        public ProjectComparisonResult Compare()
        {
            CancellationPending = false;
            var result = new ProjectComparisonResult();
            Errors = new List<Exception>();
            foreach (var table in Tables)
            {
                AssertNoCancellationPending();
                try
                {
                    result.Add(table.Compare());
                }
                catch (Exception ex)
                {
                    Errors.Add(new Exception(string.Format("Error comparing [{0}].[{1}]: ,{2}", table.SchemaName, table.TableName, ex.Message), ex));
                }
            }
            return result;
        }

        private void AssertNoCancellationPending()
        {
            if (CancellationPending)
                throw new CancellationException();
        }
        public event ProgressChangedEventHandler ProgressChanged;
        private void OnProgressChanged(ProgressChangedEventArgs e)
        {
            if (ProgressChanged != null)
                ProgressChanged(this, e);
            AssertNoCancellationPending();
        }
        private bool CancellationPending;
        

        public void Dispose()
        {
            if (ConnectionA != null)
                ConnectionA.Dispose();
            if (ConnectionB != null)
                ConnectionB.Dispose();
        }


        public void SetUserState(object state, params object[] previousResults)
        {
        }

        public object DoWork()
        {
            return Compare();
        }

        public bool SupportsCancellation
        {
            get { return true; }
        }

        public void CancelAsync()
        {
            CancellationPending = true;
        }
    }
}

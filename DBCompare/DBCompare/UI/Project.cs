using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DBCompare.Config;
using DBCompare.DAL;
using DBCompare.Model;
using System.Data.SqlClient;

namespace DBCompare.UI
{
    public partial class Project : UserControl
    {
        private DataCompareProject CompareProject;
        private MainForm MainForm { get { return (MainForm)ParentForm; } }

        public Project(Config.DataCompareProject project)
        {
            InitializeComponent();
            CompareProject = project;
            FillConnectionButton(BtnConnectionA, CompareProject.ConnectionA);
            FillConnectionButton(BtnConnectionB, CompareProject.ConnectionB);
            LoadTableList();
        }

        private void LoadTableList()
        {
            var tables = GetTables(CompareProject.ConnectionA);
            if (CompareProject.Type == ProjectType.Compare)
                FilterTableNames(tables, CompareProject.ConnectionB);
            tables.Sort();
            checkedListBox1.Items.Clear();
            var allChecked = true;
            for (int i = 0; i < tables.Count; i++)
            {
                checkedListBox1.Items.Add(tables[i]);
                var isChecked = CompareProject.ContainsTable(tables[i].SchemaName, tables[i].TableName);
                checkedListBox1.SetItemChecked(i, isChecked);
                if (!isChecked)
                    allChecked = false;
            }
            SetAllCheckedSafe(allChecked);
        }

        private List<TableNameAndSchema> GetTables(ProjectConnection projectConnection)
        {
            using (var connection = projectConnection.CreateConnection())
            {
                connection.Open();
                return DataAccess.GetTables(connection);
            }
        }

        private void FilterTableNames(List<TableNameAndSchema> tables, ProjectConnection projectConnection)
        {
            var other = GetTables(projectConnection);
            tables.RemoveAll(n => !other.Contains(n));
        }

        private void FillConnectionButton(Button BtnConnectionA, ProjectConnection connection)
        {
            BtnConnectionA.Text = connection == null ? "<Not Set>" : connection.ToString();
        }

        private void ChkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, ChkSelectAll.Checked);
            }
            LoopTables();
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(LoopTables), null);
        }
        private void LoopTables()
        {
            var allChecked = true;
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                var table = (TableNameAndSchema)checkedListBox1.Items[i];
                if (checkedListBox1.GetItemChecked(i))
                {
                    CompareProject.AddTable(table.SchemaName, table.TableName);
                }
                else
                {
                    allChecked = false;
                    CompareProject.RemoveTable(table.SchemaName, table.TableName);
                }
            }
            SetAllCheckedSafe(allChecked);
            MainForm.NotifyProjectChanged();
        }

        private void SetAllCheckedSafe(bool allChecked)
        {
            ChkSelectAll.CheckedChanged -= ChkSelectAll_CheckedChanged;
            ChkSelectAll.Checked = allChecked;
            ChkSelectAll.CheckedChanged += ChkSelectAll_CheckedChanged;
        }

        public void ChangeConnectionA()
        {
            var title = CompareProject.Type == ProjectType.Compare ? "Change Connection A" : "Change Data Source";
            var connection = CompareProject.ConnectionA;
            ChangeConnection(BtnConnectionA, connection, title);
            CompareProject.ConnectionA = connection;
        }

        public void ChangeConnectionB()
        {
            if (CompareProject.Type == ProjectType.Monitor)
                return;
            var title = "Change Connection B";
            var connection = CompareProject.ConnectionB;
            ChangeConnection(BtnConnectionB, connection, title);
            CompareProject.SetConnectionB(connection);
        }

        private void ChangeConnection(Button button, ProjectConnection connection, string title)
        {
            var builder = new SqlConnectionStringBuilder(connection.ConnectionString);
            using (var selector = new DbSelectForm())
            {
                selector.Text = title;
                selector.User = builder.IntegratedSecurity ? "" : builder.UserID;
                selector.Server = builder.DataSource;
                selector.Database = builder.InitialCatalog;
            ShowSelector:
                if (selector.ShowDialog() != DialogResult.OK)
                    return;
                if (CompareProject.Type == ProjectType.Monitor)
                {
                    if (!Environment.MachineName.Equals(selector.HostName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        MessageBox.Show("For monitoring projects, server must be on the same machine where DBCompare is running.\nThis is for backup and restore purposes.", "Invalid server", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        goto ShowSelector;
                    }
                }
                connection.ConnectionString = selector.ConnectionString;
            }
            FillConnectionButton(button, connection);
        }

        private void BtnConnectionA_Click(object sender, EventArgs e)
        {
            ChangeConnectionA();
        }

        private void BtnConnectionB_Click(object sender, EventArgs e)
        {
            ChangeConnectionB();
        }

        private void BtnRun_Click(object sender, EventArgs e)
        {
            if (CompareProject.Tables == null || CompareProject.Tables.Count == 0)
            {
                MessageBox.Show("No tables are selected", "Cannot compare", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            MainForm.Compare();
        }
    }
}

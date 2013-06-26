using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace DBCompare.UI
{
    public partial class DbSelectForm : ParentForm
    {
        public DbSelectForm()
        {
            InitializeComponent();
        }

        public string ConnectionString { get; private set; }
        public bool AllowServerChange
        {
            get { return TxtServer.Enabled; }
            set
            {
                TxtServer.Enabled = value;
                if (!value)
                {
                    TxtServer.Text = "(local)";
                }
            }
        }
        public string OkButtonText
        {
            get { return BtnOk.Text; }
            set { BtnOk.Text = value; }
        }


        public string User
        {
            get { return TxtUser.Text; }
            set { TxtUser.Text = value; }
        }

        public string Server
        {
            get { return TxtServer.Text; }
            set { TxtServer.Text = value; }
        }

        public string Database
        {
            get { return TxtDatabase.Text; }
            set { TxtDatabase.Text = value; }
        }
        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (!ValidateAll())
                return;
            var connectionString = GetConnectionString();
            if (!TryConnect(connectionString))
                return;
            ConnectionString = connectionString;
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        private bool ValidateAll()
        {
            if (string.IsNullOrEmpty(TxtServer.Text))
            {
                errorProvider1.SetError(TxtServer, "Server name cannot be empty");
                return false;
            }
            if (string.IsNullOrEmpty(TxtDatabase.Text))
            {
                errorProvider1.SetError(TxtDatabase, "Database name cannot be empty");
                return false;
            }
            return true;
        }

        private string GetConnectionString()
        {
            var connectionStringBuilder = new SqlConnectionStringBuilder();
            connectionStringBuilder.DataSource = TxtServer.Text;
            connectionStringBuilder.InitialCatalog = TxtDatabase.Text;
            if (string.IsNullOrEmpty(TxtUser.Text))
            {
                connectionStringBuilder.IntegratedSecurity = true;
            }
            else
            {
                connectionStringBuilder.UserID = TxtUser.Text;
                connectionStringBuilder.Password = TxtPassword.Text;
            }
            var connectionString = connectionStringBuilder.ConnectionString;
            return connectionString;
        }

        private bool TryConnect(string connectionString)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error connecting to database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

    }
}

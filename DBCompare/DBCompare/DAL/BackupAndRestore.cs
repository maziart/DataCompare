using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using System.IO;
using DBCompare.Actions;
using System.Data;
using System.Windows.Forms;

namespace DBCompare.DAL
{
    internal class BackupAndRestore : IAction
    {
        private string ServerName;
        private string UserName;
        private string Password;
        public BackupAndRestore(string serverName, string userName, string password)
        {
            ServerName = serverName;
            UserName = userName;
            Password = password;
        }
        private string BackupDataBase(string databaseName, string destinationPath)
        {
            ReportCurrentState(0, "Backup the {0} database!", databaseName);
            var server = GetServer();
            var backup = new Backup();
            backup.Action = BackupActionType.Database;
            backup.Database = databaseName;
            if (string.IsNullOrEmpty(Path.GetExtension(destinationPath)))  //Directory 
                destinationPath = System.IO.Path.Combine(destinationPath, databaseName + ".bak");
            Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));

            backup.Devices.Add(new BackupDeviceItem(destinationPath, DeviceType.File));
            backup.Initialize = true;
            backup.Checksum = true;
            backup.ContinueAfterError = true;
            backup.Incremental = false;
            backup.PercentCompleteNotification = 1;
            backup.LogTruncation = BackupTruncateLogType.Truncate;
            backup.PercentComplete += backup_PercentComplete;
            backup.Complete += backup_Complete;
            backup.SqlBackup(server);
            return destinationPath;
        }
        private Server Server;
        private Server GetServer()
        {
            if (Server == null)
            {
                ServerConnection connection;
                if (string.IsNullOrEmpty(UserName))
                    connection = new ServerConnection(ServerName);
                else
                    connection = new ServerConnection(ServerName, UserName, Password);
                Server = new Server(connection);
            }
            return Server;
        }
        private void backup_Complete(object sender, ServerMessageEventArgs e)
        {
            ReportCurrentState(100, e + " Completed");
        }
        private void backup_PercentComplete(object sender, PercentCompleteEventArgs e)
        {
            ReportCurrentState(e.Percent, e.Percent + "% Complete");
        }

        private void ReportCurrentState(int percentage, string operation, params object[] args)
        {
            OnProgressChanged(new ProgressChangedEventArgs(string.Format(operation, args), percentage));
        }


        void RestoreDataBase(string backupFilePath, string destinationDatabaseName, string databaseFolder)
        {
            var server = GetServer();
            var restore = new Restore();
            restore.Database = destinationDatabaseName;
            var currentDb = server.Databases[destinationDatabaseName];
            if (currentDb != null)
                server.KillAllProcesses(destinationDatabaseName);
            restore.Devices.AddDevice(backupFilePath, DeviceType.File);
            var files = restore.ReadFileList(server);
            Directory.CreateDirectory(databaseFolder);
            foreach (DataRow row in files.Rows)
            {
                var fileName = destinationDatabaseName + "_" + row["LogicalName"] + ((string)row["Type"] == "D" ? ".mdf" : ".ldf");
                restore.RelocateFiles.Add(new RelocateFile((string)row["LogicalName"], Path.Combine(databaseFolder, fileName)));
            }
            restore.ReplaceDatabase = true;
            restore.PercentCompleteNotification = 1;
            restore.PercentComplete += myRestore_PercentComplete;
            restore.Complete += myRestore_Complete;
            ReportCurrentState(0, "Restoring: {0}", destinationDatabaseName);
            restore.SqlRestore(server);
            currentDb = server.Databases[destinationDatabaseName];
            currentDb.SetOnline();
        }
        private void myRestore_Complete (object sender, ServerMessageEventArgs e)
        {
            ReportCurrentState(100, e + " Completed");
        }
        private void myRestore_PercentComplete(object sender, PercentCompleteEventArgs e)
        {
            ReportCurrentState(e.Percent, e.Percent + "% Complete");
        }

        #region IAction Members
        public event ProgressChangedEventHandler ProgressChanged;
        private void OnProgressChanged(ProgressChangedEventArgs e)
        {
            if (ProgressChanged != null)
                ProgressChanged(this, e);
        }
        private BackupRestoreUserState State;
        public void SetUserState(object state, params object[] previousResults)
        {
            State = state as BackupRestoreUserState;
            if (State == null)
                throw new ArgumentNullException("state");
            if (!State.BackUp && State.Path == null)
            {
                var path = previousResults.LastOrDefault() as string;
                if (path == null)
                    throw new ArgumentNullException("path is not provided for restore");
                State.Path = path;
            }
        }

        public object DoWork()
        {
            if (State.BackUp)
                return BackupDataBase(State.DatabaseName, State.Path);

            RestoreDataBase(State.Path, State.DatabaseName, Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Data", State.DatabaseName));
            return null;
        }

        public bool SupportsCancellation
        {
            get { return false; }
        }
        public void CancelAsync()
        {
            throw new NotSupportedException();
        }
        #endregion


        public List<Exception> Errors
        {
            get { return null; }
        }
    }
    internal class BackupRestoreUserState
    {
        public string DatabaseName { get; set; }
        public string Path { get; set; }
        public bool BackUp { get; set; }
    }
}

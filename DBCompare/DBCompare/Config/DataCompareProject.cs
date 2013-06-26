using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Text.RegularExpressions;
using DBCompare.Actions;
using DBCompare.DAL;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DBCompare.Config
{
    public class DataCompareProject
    {
        public ProjectConnection ConnectionA { get; set; }
        public ProjectConnection ConnectionB { get; set; }
        public List<Table> Tables { get; set; }
        public bool AddTable(string fullName)
        {
            return AddTable(new Table(fullName));
        }
        public bool AddTable(string schemaName, string tableName)
        {
            return AddTable(new Table(schemaName, tableName));
        }
        public bool AddTable(Table table)
        {
            if (Tables == null)
                Tables = new List<Table>();
            if (ContainsTable(table.SchemaName, table.TableName))
                return false;
            Tables.Add(table);
            IsDirty = true;
            return true;
        }

        public bool RemoveTable(string schemaName, string tableName)
        {
            if (Tables == null)
                return false;
            var result = Tables.RemoveAll(n => n.SchemaName == schemaName && n.TableName == tableName);
            if (result == 0)
                return false;
            IsDirty = true;
            return true;
        }

        public bool ContainsTable(string schemaName, string tableName)
        {
            return Tables != null && Tables.Any(n => n.SchemaName == schemaName && n.TableName == tableName);
        }

        public DataCompareProject SetConnectionA(string connectionString)
        {
            return SetConnectionA(new ProjectConnection { Provider = DataProvider.SQlServer, ConnectionString = connectionString });
        }
        public DataCompareProject SetConnectionA(ProjectConnection projectConnection)
        {
            ConnectionA = projectConnection;
            IsDirty = true;
            return this;
        }

        public DataCompareProject SetConnectionB(string connectionString)
        {
            return SetConnectionB(new ProjectConnection { Provider = DataProvider.SQlServer, ConnectionString = connectionString });
        }
        public DataCompareProject SetConnectionB(ProjectConnection projectConnection)
        {
            ConnectionB = projectConnection;
            IsDirty = true;
            return this;
        }


        [XmlAttribute]
        public ProjectType Type { get; set; }

        [XmlIgnore]
        public string FileName { get; set; }

        [XmlIgnore]
        public string Name
        {
            get
            {
                if (FileName == null)
                    return "New " + Type + " Project";
                return Path.GetFileNameWithoutExtension(FileName);
            }
        }
        [XmlIgnore]
        public bool IsDirty { get; set; }


        public static DataCompareProject LoadFromFile(string fileName)
        {
            using (var reader = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                var serializer = new XmlSerializer(typeof(DataCompareProject));
                var project = (DataCompareProject)serializer.Deserialize(reader);
                project.FileName = fileName;
                project.IsDirty = false;
                return project;
            }
        }
        public void Save()
        {
            if (FileName == null)
                throw new InvalidOperationException("FileName is not set");
            Save(FileName);
        }
        public void Save(string fileName)
        {
            if (File.Exists(fileName))
                File.Delete(fileName);
            using (var writer = new FileStream(fileName, FileMode.CreateNew, FileAccess.Write))
            {
                var serializer = new XmlSerializer(typeof(DataCompareProject));
                serializer.Serialize(writer, this);
                writer.Flush();
                writer.Close();
            }
            IsDirty = false;

            FileName = fileName;
        }

        public bool RequiresBackupRestore
        {
            get
            {
                if (Type != ProjectType.Monitor)
                    return false;
                return ConnectionB == null || !ConnectionB.TryConnect();
            }
        }

        internal void CreateBackUpAction(out IAction action, out object state)
        {
            var builderA = new SqlConnectionStringBuilder(ConnectionA.ConnectionString);
            if (ConnectionB == null)
            {
                SetConnectionB(new ProjectConnection(ConnectionA));
                ConnectionB.SetCatalog(builderA.InitialCatalog + "-" + DateTime.Now.ToString("yyyyMMddHHmm"));
            }
            action = new BackupAndRestore(builderA.DataSource, builderA.IntegratedSecurity ? null : builderA.UserID, builderA.IntegratedSecurity ? null : builderA.Password);
            state = new BackupRestoreUserState { BackUp = true, DatabaseName = builderA.InitialCatalog, Path = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Temp") };
        }

        internal void CreateRestoreAction(out IAction action, out object state)
        {
            var builderB = new SqlConnectionStringBuilder(ConnectionB.ConnectionString);
            action = new BackupAndRestore(builderB.DataSource, builderB.IntegratedSecurity ? null : builderB.UserID, builderB.IntegratedSecurity ? null : builderB.Password);
            state = new BackupRestoreUserState { BackUp = false, DatabaseName = builderB.InitialCatalog };
        }
    }
}

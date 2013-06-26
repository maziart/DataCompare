using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace DBCompare.Config
{
    public class ApplicationSettings
    {
        public List<string> RecentFiles { get; set; }
        public ApplicationSettings()
        {
            RecentFiles = new List<string>();
        }
        public static void AddRecentFile(string FileName)
        {
            Instance.AddRecentFileInternal(FileName);
        }
        private void AddRecentFileInternal(string FileName)
        {
            if (RecentFiles == null)
                RecentFiles = new List<string>();
            RecentFiles.Remove(FileName);
            while (RecentFiles.Count > 9)
                RecentFiles.RemoveAt(9);
            RecentFiles.Insert(0, FileName);
        }
        public static IEnumerable<string> GetRecentFiles()
        {
            return Instance.RecentFiles.AsEnumerable();
        }

        private static ApplicationSettings LoadFromFile(string fileName)
        {
            if (!File.Exists(fileName))
                return new ApplicationSettings();
            using (var reader = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(ApplicationSettings));
                return (ApplicationSettings)serializer.Deserialize(reader);
            }
        }
        public static void Save()
        {
            if (File.Exists(FileName))
                File.Delete(FileName);
            using (var writer = new FileStream(FileName, FileMode.CreateNew, FileAccess.Write))
            {
                var serializer = new XmlSerializer(typeof(ApplicationSettings));
                serializer.Serialize(writer, Instance);
                writer.Flush();
                writer.Close();
            }
        }

        private static ApplicationSettings Instance;
        static ApplicationSettings()
        {
            Instance = LoadFromFile(FileName);
        }
        private static string _FileName;
        private static string FileName
        {
            get
            {
                if (_FileName == null)
                {
                    var path = Path.GetDirectoryName(Application.ExecutablePath);
                    _FileName = Path.Combine(path, "Settings.xml");
                }
                return _FileName;
            }
        }
    }
}

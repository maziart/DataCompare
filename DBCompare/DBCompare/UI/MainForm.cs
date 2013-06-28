using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DBCompare.Config;
using DBCompare.Service;
using DBCompare.Comparers;
using DBCompare.Scripts;

namespace DBCompare.UI
{
    public partial class MainForm : ParentForm
    {
        public MainForm(DataCompareProject project)
        {
            InitializeComponent();
            if (project == null)
                ShowStartPage();
            else
                ShowProject(project);
            RefreshRecentFiles();
            WebService.Open();
        }
        private void LoadInnerControl(Control control)
        {
            var currentControls = MainPanel.Controls.Cast<Control>().ToList();
            MainPanel.Controls.Clear();
            currentControls.ForEach(n => n.Dispose());
            control.Dock = DockStyle.Fill;
            MainPanel.Controls.Add(control);
        }
        private bool IsDirty { get { if (CurrentProject == null) return false; return CurrentProject.IsDirty; } }
        public bool CreateNewProject(ProjectType type)
        {
            if (IsDirty)
            {
                if (!SaveCurrent())
                    return false;
            }
            var project = new DataCompareProject();
            project.Type = type;
            using (var selector = new DbSelectForm())
            {
                selector.Text = type == ProjectType.Compare ? "Select Database A" : "Select Source Database";

                selector.OkButtonText = "Next";
                selector.OkButtonImage = DBCompare.Properties.Resources.Right;
            ShowSelector:
                if (selector.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    return false;
                if (type == ProjectType.Monitor)
                {
                    if (!Environment.MachineName.Equals(selector.HostName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        MessageBox.Show("For monitoring projects, server must be on the same machine where DBCompare is running.\nThis is for backup and restore purposes.", "Invalid server", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        goto ShowSelector;
                    }
                }
                project.SetConnectionA(selector.ConnectionString);
            }
            if (type == ProjectType.Compare)
            {
                using (var selector = new DbSelectForm())
                {
                    selector.Text = "Select Database B";
                    if (selector.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                        return false;
                    project.SetConnectionB(selector.ConnectionString);
                }
            }
            else
            {
                using (var backupRestore = new BackupRestoreForm(project))
                {
                    if (backupRestore.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                        return false;
                }
            }
            return ShowProject(project);
        }
        DataCompareProject CurrentProject;
        Project CurrentProjectViewer;
        internal Compare CurrentComparer { get; private set; }
        private void ShowStartPage()
        {
            Text = "DB Compare";
            CurrentProject = null;
            LoadInnerControl(new Start());
            saveToolStripMenuItem.Enabled = closeToolStripMenuItem.Enabled = false;
            projectToolStripMenuItem.Visible = false;
        }
        public bool ShowProject(DataCompareProject project)
        {
            SetTitle(project);
            CurrentProject = project;
            CurrentProjectViewer = new Project(project);
            LoadInnerControl(CurrentProjectViewer);
            closeToolStripMenuItem.Enabled = true;
            saveToolStripMenuItem.Enabled = project.IsDirty;
            projectToolStripMenuItem.Visible = true;
            RefreshRecentFiles(project.FileName);
            return true;
        }
        private void RefreshRecentFiles(string fileName = null)
        {
            if (fileName != null)
            {
                ApplicationSettings.AddRecentFile(fileName);
                ApplicationSettings.Save();
            }
            openRecentToolStripMenuItem.DropDownItems.Clear();
            openRecentToolStripMenuItem.DropDownItems.AddRange(GetRecentFilesMenuItems().ToArray());
        }
        private IEnumerable<ToolStripMenuItem> GetRecentFilesMenuItems()
        {
            var index = 1;
            foreach (var path in ApplicationSettings.GetRecentFiles())
            {
                var name = (index == 10 ? "1&0" : ("&" + index)) + " " + GetShortenedFileName(path);
                var menu = new ToolStripMenuItem(name);
                menu.Tag = path;
                menu.Click += recentFileToolStripMenuItem_Click;
                yield return menu;
                index++;
            }
        }
        private void recentFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var menu = ((ToolStripMenuItem)sender);
            Open((string)menu.Tag);
        }
        private string GetShortenedFileName(string fileName)
        {
            if (fileName.Length <= 40)
                return fileName;
            var parts = fileName.Split('\\').ToList();
            while (parts.Count > 2 && parts.Sum(n => n.Length) + parts.Count - 1 > 37)
            {
                parts.RemoveAt(1);
            }
            if (parts.Count > 2) //the other condition if false which is desireable
            {
                parts.Insert(1, "...");
                return string.Join("\\", parts);
            }
            return fileName.Substring(0, 3) + "..." + fileName.Substring(fileName.Length - 34);
        }
        private void SetTitle(DataCompareProject project)
        {
            Text = "DB Compare - " + project.Name;
            if (project.IsDirty)
                Text += " *";
        }

        private bool SaveCurrent(bool ask = true)
        {
            if (CurrentProject == null)
                throw new InvalidOperationException("Current project is null");
            if (ask)
            {
                var choice = MessageBox.Show("Do you want to save changes to current project: " + CurrentProject.Name, "Save changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (choice == System.Windows.Forms.DialogResult.Cancel)
                    return false;
                if (choice == System.Windows.Forms.DialogResult.No)
                    return true;
            }
            if (CurrentProject.FileName == null)
            {
                using (var saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.DefaultExt = "dbcproj";
                    saveFileDialog.Filter = "DB Compare Project Files|*.dbcproj";
                    saveFileDialog.AddExtension = true;
                    saveFileDialog.FileName = CurrentProject.Name.Replace(" ", "");
                    if (saveFileDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                        return false;
                    CurrentProject.Save(saveFileDialog.FileName);
                    RefreshRecentFiles(saveFileDialog.FileName);
                }
            }
            else
            {
                CurrentProject.Save();
            }
            SetTitle(CurrentProject);
            return true;
        }

        private void compareProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateNewProject(ProjectType.Compare);
        }

        private void monitorProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateNewProject(ProjectType.Monitor);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsDirty)
            {
                if (!SaveCurrent())
                    return;
            }
            ShowStartPage();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsDirty)
            {
                if (!SaveCurrent())
                    return;
            }
            Application.Exit();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!IsDirty)
                return;
            SaveCurrent(false);
        }

        public void NotifyProjectChanged()
        {
            if (CurrentProject != null)
            {
                SetTitle(CurrentProject);
                saveToolStripMenuItem.Enabled = CurrentProject.IsDirty;
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fileName;
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.DefaultExt = "dbcproj";
                openFileDialog.Filter = "DB Compare Project Files|*.dbcproj";
                if (openFileDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    return;
                fileName = openFileDialog.FileName;
            }
            Open(fileName);
        }

        private void Open(string fileName)
        {
            if (IsDirty)
            {
                if (!SaveCurrent())
                    return;
            }
            try
            {
                var project = DataCompareProject.LoadFromFile(fileName);
                ShowProject(project);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error connecting to database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsDirty)
            {
                if (!SaveCurrent())
                    e.Cancel = true;
            }
            WebService.Close();
        }

        private void connectionAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentProjectViewer == null)
                return;
            CurrentProjectViewer.ChangeConnectionA();
        }

        private void connectionBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentProjectViewer == null)
                return;
            CurrentProjectViewer.ChangeConnectionB();
        }

        public void Compare()
        {
            CurrentComparer = new Compare(CurrentProject);
            CurrentProjectViewer = null;
            LoadInnerControl(CurrentComparer);
            CurrentComparer.StartComparing();
            closeToolStripMenuItem.Enabled = true;
            projectToolStripMenuItem.Visible = false;
        }

        private void runProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Compare();
        }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DBCompare.UI
{
    public partial class Start : UserControl
    {
        public Start()
        {
            InitializeComponent();
        }
        private MainForm MainForm { get { return (MainForm)ParentForm; } }
        private void BtnCompare_Click(object sender, EventArgs e)
        {
            MainForm.CreateNewProject(Config.ProjectType.Compare);
        }

        private void BtnMonitor_Click(object sender, EventArgs e)
        {
            MainForm.CreateNewProject(Config.ProjectType.Monitor);
        }
    }
}

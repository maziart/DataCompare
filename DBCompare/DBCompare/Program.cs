using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;
using DBCompare.Comparers;
using DBCompare.Config;
using DBCompare.UI;
using DBCompare.Service;

namespace DBCompare
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(params string[] args)
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                DataCompareProject project = null;
                if (args.Length == 1)
                {
                    project = DataCompareProject.LoadFromFile(args[0]);
                }
                var mainForm = ServiceAccess.MainForm = new MainForm(project);
                Application.Run(mainForm);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

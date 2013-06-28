using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;
using DBCompare.Comparers;
using DBCompare.Config;
using DBCompare.UI;
using DBCompare.Service;
using System.ServiceModel;

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
                ApplicationArguments.Initialize(args);

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                DataCompareProject project = null;
                var fileName = ApplicationArguments.Instance[ArgumentType.Open];
                if (fileName != null)
                {
                    project = DataCompareProject.LoadFromFile(fileName);
                }
                var mainForm = ServiceAccess.MainForm = new MainForm(project);
                Application.Run(mainForm);
            }
            catch (AddressAlreadyInUseException ex) //TODO
            {
                ShowError("Failed to register Http Server.\nYou can change the port in .config file or provide argument: Port=[Port#]\n\n" + ex.Message);
            }
            catch (Exception ex)
            {
                ShowError(ex.GetBaseException().Message);
            }
        }
        static void ShowError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}

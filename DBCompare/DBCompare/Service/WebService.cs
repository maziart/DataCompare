using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Web;
using System.ServiceModel.Description;
using System.ServiceModel;
using System.IO;
using DBCompare.Config;

namespace DBCompare.Service
{
    public class WebService : IWebService
    {
        private WebService()
        {

        }
        public Stream Ping(string name)
        {
            var response = new PingResponse(name);
            response.Flush();
            return response.Stream;
        }

        public Stream GetFile(string name)
        {
            var response = new FileResponse(name);
            response.Flush();
            return response.Stream;
        }

        public Stream GetFileWithDirectory(string directory, string name)
        {
            return GetFile(directory + "." + name);
        }

        public Stream GetComparisonDetails(string schemaName, string tableName)
        {
            var response = new ComparisonDetailsResponse(schemaName, tableName);
            response.Flush();
            return response.Stream;
        }

        public Stream GetScripts()
        {
            var response = new ScriptResponse();
            response.Flush();
            return response.Stream;
        }

        public Stream GetScript(string schemaName, string tableName)
        {
            var response = new ScriptResponse(schemaName, tableName);
            response.Flush();
            return response.Stream;
        }

        public Stream GetFavIcon()
        {
            var response = new FileResponse("Favicon.ico");
            response.Flush();
            return response.Stream;
        }
        #region Host Open/Close
        private static WebServiceHost Host;
        public static void Open()
        {
            var baseUrl = ":" + Configuration.WebServicePort + "/";
            var host = new WebServiceHost(typeof(WebService), new Uri("http://localhost" + baseUrl));
            BaseUrl = "http://" + Environment.MachineName + baseUrl;
            var ep = host.AddServiceEndpoint(typeof(IWebService), new WebHttpBinding(), "");
            var sdb = host.Description.Behaviors.Find<ServiceDebugBehavior>();
            sdb.HttpHelpPageEnabled = false;
            host.Open();
            Host = host;
        }
        public static void Close()
        {
            if (Host != null)
                Host.Close();
        }
        public static string BaseUrl { get; private set; }
        public static string GetFileUrl(string fileName)
        {
            return BaseUrl + "Files/" + fileName;
        }
        public static string GetComparisonDetailsUrl(string schemaName, string tableName)
        {
            return BaseUrl + "Details/" + schemaName + "/" + tableName;
        }
        public static string GetScriptsUrl()
        {
            return BaseUrl + "Script";
        }
        #endregion
    }
}

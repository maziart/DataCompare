using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.IO;

namespace DBCompare.Service
{
    [ServiceContract]
    public interface IWebService
    {
        [OperationContract]
        [WebGet(UriTemplate="Ping/{name}")]
        Stream Ping(string name);


        [OperationContract]
        [WebGet(UriTemplate = "Files/{name}")]
        Stream GetFile(string name);

        [OperationContract]
        [WebGet(UriTemplate = "Files/{directory}/{name}")]
        Stream GetFileWithDirectory(string directory, string name);

        [OperationContract]
        [WebGet(UriTemplate = "Details/{schemaName}/{tableName}")]
        Stream GetComparisonDetails(string schemaName, string tableName);

        [OperationContract]
        [WebGet(UriTemplate = "Script")]
        Stream GetScripts();

        [OperationContract]
        [WebGet(UriTemplate = "Script/{schemaName}/{tableName}")]
        Stream GetScript(string schemaName, string tableName);

        [OperationContract]
        [WebGet(UriTemplate = "Favicon.ico")]
        Stream GetFavIcon();
    }
}

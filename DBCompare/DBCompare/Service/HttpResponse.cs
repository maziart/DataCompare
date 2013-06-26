using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Web;
using System.IO;

namespace DBCompare.Service
{
    internal abstract class HttpResponse
    {
        public Stream Stream { get; protected set; }
        public TextWriter Writer { get; protected set; }
        public HttpResponse()
        {
            OnInit();
        }
        protected virtual void OnInit()
        {
            Stream = new MemoryStream();
            Writer = new StreamWriter(Stream);
        }
        protected virtual void Render()
        {
        }
        protected void NotFound()
        {
            WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.NotFound;
            Stream = new MemoryStream();
        }
        protected abstract string ContentType { get; }
        public void Flush()
        {
            Render();
            if (Writer != null)
                Writer.Flush();
            if (Stream != null)
                Stream.Position = 0;
            WebOperationContext.Current.OutgoingResponse.ContentType = ContentType;
        }
    }
}

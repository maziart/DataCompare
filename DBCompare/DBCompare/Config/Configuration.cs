using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace DBCompare.Config
{
    public static class Configuration
    {
        public static int WebServicePort
        {
            get
            {
                var arg = ApplicationArguments.Instance[ArgumentType.Port] ?? ConfigurationManager.AppSettings["WebServicePort"];
                int port;
                if (!int.TryParse(arg, out port))
                    port = 457;
                return port;
            }
        }
    }
}

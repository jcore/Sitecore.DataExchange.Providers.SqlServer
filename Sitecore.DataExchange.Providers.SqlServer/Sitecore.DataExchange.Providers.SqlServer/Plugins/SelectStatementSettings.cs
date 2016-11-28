using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.DataExchange.Providers.SqlServer.Plugins
{
    public class SelectStatementSettings : Sitecore.DataExchange.IPlugin
    {
        public SelectStatementSettings() { }

        public string SelectStatement { get; set; }
        public string ConnectionString { get; set; }
  }
}

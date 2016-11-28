using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Providers.SqlServer.Plugins;

namespace Sitecore.DataExchange.Providers.SqlServer
{
  public static class EndpointExtensions
    {
        public static SelectStatementSettings GetSelectStatementSettings(this Endpoint endpoint)
        {
            return endpoint.GetPlugin<SelectStatementSettings>();
        }
        public static bool HasSelectStatementSettings(this Endpoint endpoint)
        {
            return (GetSelectStatementSettings(endpoint) != null);
        }
    }
}

using Sitecore.DataExchange.Converters.Endpoints;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using System;
using Sitecore.DataExchange.Providers.SqlServer.Models.ItemModels.Endpoints;
using Sitecore.DataExchange.Providers.SqlServer.Plugins;

namespace Sitecore.DataExchange.Providers.SqlServer.Converters.Endpoints
{
  public class SelectStatementEndpointConverter : BaseEndpointConverter<ItemModel>
    {
        private static readonly Guid TemplateId = Guid.Parse("{EA64AA4F-7EEE-4CE8-BB3B-6C18AB790760}");
        public SelectStatementEndpointConverter(IItemModelRepository repository)
            : base(repository)
        {
            //
            //identify the template an item must be based
            //on in order for the converter to be able to
            //convert the item
            this.SupportedTemplateIds.Add(TemplateId);
        }
        protected override void AddPlugins(ItemModel source, Endpoint endpoint)
        {
            //
            //create the plugin
            var settings = new SelectStatementSettings();
            //
            //populate the plugin using values from the item
            settings.SelectStatement =
                base.GetStringValue(source, SelectStatementEndpointItemModel.SelectStatement);
            settings.ConnectionString =
                      base.GetStringValue(source, SelectStatementEndpointItemModel.ConnectionString);
            //
            //add the plugin to the endpoint
            endpoint.Plugins.Add(settings);
        }
    }
}

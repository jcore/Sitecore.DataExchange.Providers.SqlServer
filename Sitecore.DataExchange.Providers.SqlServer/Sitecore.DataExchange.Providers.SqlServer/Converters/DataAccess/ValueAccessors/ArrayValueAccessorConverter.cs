﻿using Sitecore.DataExchange.Converters.DataAccess.ValueAccessors;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.DataAccess.Readers;
using Sitecore.DataExchange.DataAccess.Writers;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using System;
using Sitecore.DataExchange.Providers.SqlServer.Models.ItemModels.DataAccess;

namespace Sitecore.DataExchange.Providers.SqlServer.Converters.DataAccess.ValueAccessors
{
  public class ArrayValueAccessorConverter : ValueAccessorConverter
  {
    private static readonly Guid TemplateId = Guid.Parse("{F344B280-0C3F-44BE-9BC7-93ED49C04FC2}");
    public ArrayValueAccessorConverter(IItemModelRepository repository) : base(repository)
    {
      this.SupportedTemplateIds.Add(TemplateId);
    }
    public override IValueAccessor Convert(ItemModel source)
    {
      var accessor = base.Convert(source);
      if (accessor == null)
      {
        return null;
      }
      var position = base.GetIntValue(source, ArrayValueAccessorItemModel.Position);
      if (position <= 0)
      {
        return null;
      }
      //
      //array position is 1-based in the Sitecore item but is 0-based in the reader/writer
      position--;
      //
      //unless a reader or writer is explicitly set use the property value
      if (accessor.ValueReader == null)
      {
        accessor.ValueReader = new ArrayValueReader(position);
      }
      if (accessor.ValueWriter == null)
      {
        accessor.ValueWriter = new ArrayValueWriter(position);
      }
      return accessor;
    }
  }
}
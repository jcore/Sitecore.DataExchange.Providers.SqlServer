using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Processors.PipelineSteps;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Sitecore.DataExchange.Providers.SqlServer.Plugins;

namespace Sitecore.DataExchange.Providers.SqlServer.Processors.PipelineSteps
{
  [RequiredEndpointPlugins(typeof(SelectStatementSettings))]
  public class RunSelectStatementStepProcessor : BaseReadDataStepProcessor
  {
    public RunSelectStatementStepProcessor()
    {
    }
    public override bool CanProcess(PipelineStep pipelineStep, PipelineContext pipelineContext)
    {
      return base.CanProcess(pipelineStep, pipelineContext);
    }
    public override void Process(PipelineStep pipelineStep, PipelineContext pipelineContext)
    {
      base.Process(pipelineStep, pipelineContext);
    }
    protected override void ReadData(Endpoint endpoint, PipelineStep pipelineStep, PipelineContext pipelineContext)
    {
      if (endpoint == null)
      {
        throw new ArgumentNullException(nameof(endpoint));
      }
      if (pipelineStep == null)
      {
        throw new ArgumentNullException(nameof(pipelineStep));
      }
      if (pipelineContext == null)
      {
        throw new ArgumentNullException(nameof(pipelineContext));
      }
      var logger = pipelineContext.PipelineBatchContext.Logger;
      //
      //get the file path from the plugin on the endpoint
      var settings = endpoint.GetSelectStatementSettings();
      if (settings == null)
      {
        logger.Error("No text file settings are specified on the endpoint. (pipeline step: {0}, endpoint: {1})", pipelineStep.Name, endpoint.Name);
        return;
      }
      if (string.IsNullOrWhiteSpace(settings.SelectStatement))
      {
        logger.Error("No Select Statement is specified on the endpoint. (pipeline step: {0}, endpoint: {1})", pipelineStep.Name, endpoint.Name);
        return;
      }
      if (string.IsNullOrWhiteSpace(settings.ConnectionString))
      {
        logger.Error("No ConnectionString is specified on the endpoint. (pipeline step: {0}, endpoint: {1})", pipelineStep.Name, endpoint.Name);
        return;
      }
      //
      var rows = new List<string[]>();
      using (SqlConnection connection = new SqlConnection(settings.ConnectionString))
      {
        var commandText = settings.SelectStatement;
        using (SqlCommand command = new SqlCommand(commandText, connection))
        {
          connection.Open();
          using (SqlDataAdapter dataAdapter = new SqlDataAdapter())
          {
            var dataSet = new DataSet();
            dataAdapter.SelectCommand = command;
            dataAdapter.Fill(dataSet);
            if (dataSet.Tables != null && dataSet.Tables.Count > 0)
            {
              var table = dataSet.Tables[0];
              for (var i = 0; i < table.Rows.Count; i++)
              {
                var row = table.Rows[i];
                var rowValues = new string[table.Columns.Count];
                for (var c = 0; c < table.Columns.Count; c++)
                {
                  if (((DataColumn)row[c]).DataType == typeof(DateTime))
                  {
                    rowValues[c] = DateUtil.ToIsoDate(System.Convert.ToDateTime(row[c]));
                  }
                  else
                  {
                    rowValues[c] = row[c].ToString();
                  }
                }
                rows.Add(rowValues);
              }
            }
          }
        }
        var dataSettings = new IterableDataSettings(rows);
        logger.Info("{0} rows were read from the file. (pipeline step: {1}, endpoint: {2})", rows.Count, pipelineStep.Name, endpoint.Name);
        //
        //add the plugin to the pipeline context
        pipelineContext.Plugins.Add(dataSettings);
      }
    }
  }
}

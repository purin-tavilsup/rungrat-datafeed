using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using RungratDataFeed.Extensions;
using RungratDataFeed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace RungratDataFeed.Functions
{
    public static class GetSalesReportById
    {
        [FunctionName("GetSalesReportById")]
        public static async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Function, "get", Route = "datafeed/salesreports/{id}")] HttpRequest req,
			string id,
			[CosmosDB(databaseName: Constants.DatabaseId,
					  collectionName: Constants.InvoiceContainerId,
					  ConnectionStringSetting = "CosmosDbConnectionString")] IDocumentClient client,
            ILogger log)
        {
			try
			{
				var reportId = id.HasValue() ? id : $"{DateTime.UtcNow.Year}";

				log.LogInformation($"Getting sales report by id: {reportId}");

				var report = await GetReport(reportId, client);

				if (report == null)
					return new NotFoundObjectResult($"Sales report with id '{reportId}' was not found.");

				return new OkObjectResult(report);
			}
			catch (Exception ex)
			{
				log.LogCritical(ex, "An error occurred.");

				return new ExceptionResult(ex, includeErrorDetail: true);
			}
        }

		private static async Task<SalesReport> GetReport(string reportId, IDocumentClient client)
		{
			var sqlCommand = $"SELECT * FROM salesReports r WHERE r.id = '{reportId}'";
			var containerUri = UriFactory.CreateDocumentCollectionUri(Constants.DatabaseId, Constants.SalesReportContainerId);
			var query = client.CreateDocumentQuery<SalesReport>(containerUri, sqlCommand)
							  .AsDocumentQuery();

			var reports = new List<SalesReport>();

			while (query.HasMoreResults)
			{
				foreach (SalesReport result in await query.ExecuteNextAsync())
				{
					reports.Add(result);
				}
			}

			return reports.FirstOrDefault();
		}
    }
}

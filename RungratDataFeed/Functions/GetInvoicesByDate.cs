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
using System.Threading.Tasks;
using System.Web.Http;

namespace RungratDataFeed.Functions
{
    public static class GetInvoicesByDate
    {
        [FunctionName("GetInvoicesByDate")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "datafeed/invoices")] HttpRequest req,
			[CosmosDB(databaseName: Constants.DatabaseId,
					  collectionName: Constants.InvoiceContainerId,
					  ConnectionStringSetting = "CosmosDbConnectionString")] IDocumentClient client,
            ILogger log)
        {
            try
			{
				var date = GetDateParameter(req);

				log.LogInformation($"Getting invoices by date: {date}");

				var invoices = await GetInvoices(date, client);

				return new OkObjectResult(invoices);
			}
			catch (Exception ex)
			{
				log.LogCritical(ex, "An error occurred.");

				return new ExceptionResult(ex, includeErrorDetail: true);
			}
        }

		private static string GetDateParameter(HttpRequest req)
        {
			string dateParameter = req.Query["date"];

			return dateParameter.HasValue() ? dateParameter : $"{DateTime.UtcNow:yyyy-M-d}";
		}

		private static async Task<Invoice[]> GetInvoices(string date, IDocumentClient client)
        {
			var sqlCommand = $"SELECT * FROM invoices i WHERE i.date = '{date}'";
			var containerUri = UriFactory.CreateDocumentCollectionUri(Constants.DatabaseId, Constants.InvoiceContainerId);
			var query = client.CreateDocumentQuery<Invoice>(containerUri, sqlCommand)
							  .AsDocumentQuery();

			var invoices = new List<Invoice>();

			while (query.HasMoreResults)
			{
				foreach (Invoice result in await query.ExecuteNextAsync())
				{
					invoices.Add(result);
				}
			}

			return invoices.ToArray();
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using RungratDataFeed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace RungratDataFeed.Functions
{
    public static class GetInvoicesByDate
    {
        [FunctionName("GetInvoicesByDate")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "invoices")] HttpRequest req,
			[CosmosDB(databaseName: "rungrat-datafeed-cosmos-db",
					  collectionName: "Invoices",
					  ConnectionStringSetting = "CosmosDbConnectionString")] IDocumentClient client,
            ILogger log)
        {
            try
			{
				string date = req.Query["Date"];

				if (string.IsNullOrWhiteSpace(date))
				{
					return new NotFoundResult();
				}

				var collectionUri = UriFactory.CreateDocumentCollectionUri("rungrat-datafeed-cosmos-db", "Invoices");

				log.LogInformation($"Searching by Date: {date}");

				var query = client.CreateDocumentQuery<Invoice>(collectionUri)
								  .Where(invoice => invoice.Date == date)
								  .AsDocumentQuery();

				var invoices = new List<Invoice>();

				while (query.HasMoreResults)
				{
					foreach (Invoice result in await query.ExecuteNextAsync())
					{
						invoices.Add(result);
					}
				}

				return new OkObjectResult(invoices);
			}
			catch (Exception ex)
			{
				log.LogCritical(ex, "An error occurred while querying invoices.");

				return new ExceptionResult(ex, includeErrorDetail: true);
			}
        }
    }
}

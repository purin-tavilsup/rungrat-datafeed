using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using RungratDataFeed.Extensions;
using RungratDataFeed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace RungratDataFeed.Functions
{
    public static class GetInvoiceProductsByDate
    {
		[FunctionName("GetInvoiceProductsByDate")]
		public static async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Function, "get", Route = "datafeed/invoiceProducts")] HttpRequest req,
			[CosmosDB(databaseName: Constants.DatabaseId,
					  collectionName: Constants.InvoiceContainerId,
					  ConnectionStringSetting = "CosmosDbConnectionString")] IDocumentClient client,
			ILogger log)
		{
			try
			{
				var date = GetDateParameter(req);
				var group = GetGroupParameter(req);

				log.LogInformation($"Getting products by date: {date}");

				var products = await GetProducts(client, date, group);

				return new OkObjectResult(products);
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

		private static string GetGroupParameter(HttpRequest req)
		{
			return req.Query["group"];
		}

		private static async Task<Product[]> GetProducts(IDocumentClient client, string date, string group)
		{
			var sqlCommand = $"SELECT i.products FROM invoices i WHERE i.date = '{date}'";
			var containerUri = UriFactory.CreateDocumentCollectionUri(Constants.DatabaseId, Constants.InvoiceContainerId);
			var query = client.CreateDocumentQuery<Product>(containerUri, sqlCommand)
							  .AsDocumentQuery();

			var products = new List<Product>();

			while (query.HasMoreResults)
			{
				foreach (var result in await query.ExecuteNextAsync())
				{
					JArray productsInJsonArray = result.products;

					products.AddRange(group.HasValue()
										  ? productsInJsonArray.ToObject<IEnumerable<Product>>().Where(p => p.Group == group)
										  : productsInJsonArray.ToObject<IEnumerable<Product>>());
				}
			}

			return products.ToArray();
		}
    }
}

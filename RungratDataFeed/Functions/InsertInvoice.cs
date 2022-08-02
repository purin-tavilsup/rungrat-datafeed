using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using RungratDataFeed.Models;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web.Http;

namespace RungratDataFeed.Functions
{
    public static class InsertInvoice
    {
        [FunctionName("InsertInvoice")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "invoices")] HttpRequest req,
			[CosmosDB(databaseName: "rungrat-datafeed-cosmos-db",
					  collectionName: "Invoices",
					  ConnectionStringSetting = "CosmosDbConnectionString")] IAsyncCollector<Invoice> invoices,
            ILogger log)
        {
            log.LogInformation("Received a request to insert invoice.");

			try
			{
				var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
				var invoice = JsonSerializer.Deserialize<Invoice>(requestBody);

				if (invoice == null)
					return new BadRequestErrorMessageResult("Invoice is invalid.");

				await invoices.AddAsync(invoice);

				log.LogInformation($"Invoice:{invoice.InvoiceId} was inserted successfully");

				return new OkResult();
			}
			catch (Exception ex)
			{
				log.LogCritical(ex, "An error occurred while inserting the invoice.");

				return new ExceptionResult(ex, includeErrorDetail: true);
			}
		}
    }
}

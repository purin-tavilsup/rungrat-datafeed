using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RungratDataFeed.Extensions;
using RungratDataFeed.Models;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;

namespace RungratDataFeed.Functions
{
    public static class InsertInvoice
    {
        [FunctionName("InsertInvoice")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "datafeed/invoices")] HttpRequest req,
			[CosmosDB(databaseName: Constants.DatabaseId,
					  collectionName: Constants.InvoiceContainerId,
					  ConnectionStringSetting = "CosmosDbConnectionString")] IAsyncCollector<Invoice> invoices,
            ILogger log)
        {
			try
			{
				var invoice = await CreateInvoice(req);

				await invoices.AddAsync(invoice);

				log.LogInformation($"Invoice:{invoice.InvoiceId} was inserted successfully");

				return new OkResult();
			}
			catch (Exception ex)
			{
				log.LogCritical(ex, "An error occurred.");

				return new ExceptionResult(ex, includeErrorDetail: true);
			}
		}

		private static async Task<Invoice> CreateInvoice(HttpRequest req)
        {
			var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

			if (requestBody.HasValue()) 
				return JsonConvert.DeserializeObject<Invoice>(requestBody);

			throw new ArgumentNullException(requestBody,"Request body cannot be null or empty.");
		}
    }
}

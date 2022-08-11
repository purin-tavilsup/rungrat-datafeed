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
    public static class InsertSalesReport
    {
        [FunctionName("InsertSalesReport")]
		public static async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Function, "post", Route = "datafeed/salesreports")] HttpRequest req,
			[CosmosDB(databaseName: Constants.DatabaseId,
					  collectionName: Constants.SalesReportContainerId,
					  ConnectionStringSetting = "CosmosDbConnectionString")] IAsyncCollector<SalesReport> reports,
            ILogger log)
        {
			try
			{
				var report = await CreateReport(req);

				await reports.AddAsync(report);

				log.LogInformation($"Report:{report.Id} was inserted/updated successfully");

				return new OkResult();
			}
			catch (Exception ex)
			{
				log.LogCritical(ex, "An error occurred.");

				return new ExceptionResult(ex, includeErrorDetail: true);
			}
        }

		private static async Task<SalesReport> CreateReport(HttpRequest req)
		{
			var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

			if (!requestBody.HasValue())
				throw new ArgumentNullException(requestBody, "Request body cannot be null or empty.");

			var report = JsonConvert.DeserializeObject<SalesReport>(requestBody);

			report.Id = $"{DateTime.UtcNow.Year}";

			return report;
		}
    }
}

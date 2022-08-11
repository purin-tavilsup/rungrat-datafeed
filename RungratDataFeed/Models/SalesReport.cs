using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Diagnostics.CodeAnalysis;

namespace RungratDataFeed.Models
{
    [ExcludeFromCodeCoverage]
	[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class SalesReport
    {
		public string Id { get; set; }
		public SaleSummary DaySummary { get; set; }
		public SaleSummary MonthSummary { get; set; }
		public SaleSummary YearSummary { get; set; }
	}
}

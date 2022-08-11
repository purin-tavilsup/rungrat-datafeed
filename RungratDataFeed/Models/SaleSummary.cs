using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Diagnostics.CodeAnalysis;

namespace RungratDataFeed.Models
{
    [ExcludeFromCodeCoverage]
	[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class SaleSummary
    {
		public double InvoiceTotal { get; set; }
		public double GeneralProductsTotal { get; set; }
		public double HardwareProductsTotal { get; set; }
		public double ArTotal { get; set; }
		public double ArTotalForGeneralProducts { get; set; }
		public double ArTotalForHardwareProducts { get; set; }
		public double InvoiceTotalWithoutAr { get; set; }
		public double GeneralProductsTotalWithoutAr { get; set; }
		public double HardwareProductsTotalWithoutAr { get; set; }
    }
}

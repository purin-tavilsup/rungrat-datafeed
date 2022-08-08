using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Diagnostics.CodeAnalysis;

namespace RungratDataFeed.Models
{
    [ExcludeFromCodeCoverage]
	[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class Invoice
    {
        public int InvoiceId { get; set; }
        public string Date { get; set; }
        public DateTime DateTime { get; set; }
        public Product[] Products { get; set; }
        public Payment[] Payments { get; set; }
        public double InvoiceTotal { get; set; }
        public double GeneralProductsTotal { get; set; }
        public double HardwareProductsTotal { get; set; }
        public double ArTotal { get; set; }
        public double ArTotalForGeneralProducts { get; set; }
        public double ArTotalForHardwareProducts { get; set; }
	}
}

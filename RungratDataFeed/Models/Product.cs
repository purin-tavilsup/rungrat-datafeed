using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Diagnostics.CodeAnalysis;

namespace RungratDataFeed.Models
{
    [ExcludeFromCodeCoverage]
	[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class Product
    {
        public int ProductId { get; set; }
        public string Description { get; set; }
        public string Barcode { get; set; }
        public string Category { get; set; }
        public string Group { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string Note { get; set; }
		public DateTime DateTime { get; set; }
    }
}

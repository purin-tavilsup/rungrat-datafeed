using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Diagnostics.CodeAnalysis;

namespace RungratDataFeed.Models
{
    [ExcludeFromCodeCoverage]
	[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class Payment
    {
        public int PaymentId { get; set; }
        public string PaymentType { get; set; }
        public int PaymentTypeId { get; set; }
        public double Amount { get; set; }
        public string Note { get; set; }
		public DateTime DateTime { get; set; }
    }
}

using System;
using System.Diagnostics.CodeAnalysis;

namespace RungratDataFeed.Models
{
    [ExcludeFromCodeCoverage]
    public class Payment
    {
        public int paymentId { get; set; }
        public string paymentType { get; set; }
        public int paymentTypeId { get; set; }
        public double amount { get; set; }
        public string note { get; set; }
		public DateTime dateTime { get; set; }
    }
}

using System;
using System.Diagnostics.CodeAnalysis;

namespace RungratDataFeed.Models
{
    [ExcludeFromCodeCoverage]
    public class Product
    {
        public int productId { get; set; }
        public string description { get; set; }
        public string barcode { get; set; }
        public string category { get; set; }
        public int categoryId { get; set; }
        public double unitPrice { get; set; }
        public int quantity { get; set; }
        public string note { get; set; }
		public DateTime dateTime { get; set; }
    }
}

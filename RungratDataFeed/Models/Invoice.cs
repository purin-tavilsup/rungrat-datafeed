using System;
using System.Diagnostics.CodeAnalysis;

namespace RungratDataFeed.Models
{
    [ExcludeFromCodeCoverage]
    public class Invoice
    {
        public int invoiceId { get; set; }
        public int year { get; set; }
        public int month { get; set; }
        public int day { get; set; }
        public string date { get; set; }
        public DateTime dateTime { get; set; }
        public Product[] products { get; set; }
        public Payment[] payments { get; set; }
        public double invoiceTotal { get; set; }
        public double generalProductsTotal { get; set; }
        public double hardwareProductsTotal { get; set; }
        public double arTotal { get; set; }
        public double arTotalForGeneralProducts { get; set; }
        public double arTotalForHardwareProducts { get; set; }
	}
}

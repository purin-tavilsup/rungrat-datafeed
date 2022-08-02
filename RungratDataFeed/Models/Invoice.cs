using System.Text.Json.Serialization;

namespace RungratDataFeed.Models
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public string Date { get; set; }
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

namespace RungratDataFeed.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Description { get; set; }
        public string Barcode { get; set; }
        public string Category { get; set; }
        public int CategoryId { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string Note { get; set; }
    }
}

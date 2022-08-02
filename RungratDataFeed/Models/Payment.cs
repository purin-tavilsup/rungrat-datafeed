namespace RungratDataFeed.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public string PaymentType { get; set; }
        public int PaymentTypeId { get; set; }
        public double Amount { get; set; }
        public string Note { get; set; }
    }
}

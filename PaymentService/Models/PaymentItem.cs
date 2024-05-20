namespace PaymentService.Models
{
    public class PaymentItem
    {
        public int UserId { get; set; }
        public int OrderId { get; set; }
        public decimal Price { get; set; }
    }
}

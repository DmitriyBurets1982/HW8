namespace DeliveryService.Models;

public class DeliveryItem
{
    public int UserId { get; set; }
    public int OrderId { get; set; }
    public DateTime DeliveryDate { get; set; }  
}

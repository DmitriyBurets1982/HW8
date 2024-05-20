namespace Contracts.Saga;

public class ReserveDeliveryRequest : RequestBase
{
    public DateTime DeliveryDate { get; set; }
}

namespace Contracts.Saga;

public class CreateOrderRequest : RequestBase
{
    public decimal Price { get; set; }
}

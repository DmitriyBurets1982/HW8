namespace Contracts.Saga;

public class CreatePaymentRequest : RequestBase
{
    public decimal Price { get; set; }
}

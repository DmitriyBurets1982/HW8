namespace Contracts.Saga;

public abstract class RequestBase
{
    public int UserId { get; set; }
    public int OrderId { get; set; }
    public ServiceError? ServiceError { get; set; }
}

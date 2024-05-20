namespace Contracts.Saga;

public abstract class ResponseBase
{
    public int UserId { get; set; }
    public int OrderId { get; set; }
    public string? ErrorMessage { get; set; }
}

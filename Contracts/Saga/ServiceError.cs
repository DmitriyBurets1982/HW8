namespace Contracts.Saga;

/// <summary>
/// Used for an error simulation in the specified service
/// </summary>
public enum ServiceError
{
    None = 0,
    Payment,
    Stock,
    Delivery
}

using Contracts.Saga;
using MassTransit;

namespace OrderService.Saga;

public class OrderStateInstance : SagaStateMachineInstance
{
    /// <summary>
    /// Unique id for saga
    /// </summary>
    public Guid CorrelationId { get; set; }

    /// <summary>
    /// Unique request id from request client - need for send response
    /// </summary>
    public Guid? RequestId { get; set; }
    public int UserId { get; set; }
    public int OrderId { get; set; }
    public decimal Price { get; set; }
    public ServiceError? ServiceError { get; set; }

    /// <summary>
    /// Saga state
    /// </summary>
    public int CurrentState { get; set; }
    public Uri? ResponseAddress { get; set; }
}

using Contracts.Saga;
using MassTransit;

namespace OrderService.Saga;

public class OrderStateMachine : MassTransitStateMachine<OrderStateInstance>
{
    public Request<OrderStateInstance, CreatePaymentRequest, CreatePaymentResponse> CreatePayment { get; set; }
    public Request<OrderStateInstance, ReserveStockRequest, ReserveStockResponse> ReserveStock { get; set; }
    public Request<OrderStateInstance, ReserveDeliveryRequest, ReserveDeliveryResponse> ReserveDelivery { get; set; }
    public Event<CreateOrderRequest> CreateOrder { get; set; }

    private readonly ILogger<OrderStateMachine> _logger;

    public OrderStateMachine(ILogger<OrderStateMachine> logger)
    {
        _logger = logger;

        InstanceState(x => x.CurrentState);

        Event(() => CreateOrder, x => x.CorrelateById(y => y.OrderId, z => z.Message.OrderId)
            .SelectId(context => Guid.NewGuid()));

        Request(() => CreatePayment, r => { r.Timeout = TimeSpan.Zero; });
        Request(() => ReserveStock, r => { r.Timeout = TimeSpan.Zero; });
        Request(() => ReserveDelivery, r => { r.Timeout = TimeSpan.Zero; });

        Initially(
            When(CreateOrder)
            .Then(x =>
            {
                if (!x.TryGetPayload(out SagaConsumeContext<OrderStateInstance, CreateOrderRequest> payload))
                    throw new Exception("Unable to retrieve required payload for callback data.");
                
                x.Saga.RequestId = payload.RequestId;
                x.Saga.ResponseAddress = payload.ResponseAddress;
                x.Saga.UserId = payload.Message.UserId;
                x.Saga.OrderId = payload.Message.OrderId;
                x.Saga.Price = payload.Message.Price;
                x.Saga.ServiceError = payload.Message.ServiceError;
            })
            .Request(CreatePayment, x => x.Init<CreatePaymentRequest>(new { x.Message.UserId, x.Message.OrderId, x.Message.Price, x.Message.ServiceError }))
            .TransitionTo(CreatePayment!.Pending));

        During(CreatePayment.Pending,

            When(CreatePayment.Completed)
            .Request(ReserveStock, x => x.Init<ReserveStockRequest>(new { x.Message.UserId, x.Message.OrderId, x.Message.Price, x.Saga.ServiceError }))
            .TransitionTo(ReserveStock!.Pending),

            When(CreatePayment.Faulted)
              .ThenAsync(async context =>
              {
                  await DiscardChanges(context);
                  await RespondFromSaga(context, "Faulted On Create Payment: " + string.Join("; ", context.Message.Exceptions.Select(x => x.Message)));
              })
              .Finalize(),

            When(CreatePayment.TimeoutExpired)
               .ThenAsync(async context =>
               {
                   await DiscardChanges(context);
                   await RespondFromSaga(context, "Timeout Expired On Create Payment");
               })
               .Finalize());

        During(ReserveStock.Pending,

            When(ReserveStock.Completed)
            .Request(ReserveDelivery, x => x.Init<ReserveDeliveryRequest>(new ReserveDeliveryRequest { UserId = x.Message.UserId, OrderId = x.Message.OrderId, ServiceError = x.Saga.ServiceError, DeliveryDate = DateTime.UtcNow }))
            .TransitionTo(ReserveDelivery!.Pending),

            When(ReserveStock.Faulted)
              .ThenAsync(async context =>
              {
                  await DiscardChanges(context);
                  await RespondFromSaga(context, "Faulted On Reserve Stock: " + string.Join("; ", context.Message.Exceptions.Select(x => x.Message)));
              })
              .Finalize(),

            When(ReserveStock.TimeoutExpired)
               .ThenAsync(async context =>
               {
                   await DiscardChanges(context);
                   await RespondFromSaga(context, "Timeout Expired On Reserve Stock");
               })
               .Finalize());

        During(ReserveDelivery.Pending,

            When(ReserveDelivery.Completed)
              .ThenAsync(async context =>
              {
                  await RespondFromSaga(context, null);
              })
              .Finalize(),

            When(ReserveDelivery.Faulted)
              .ThenAsync(async context =>
              {
                  await DiscardChanges(context);
                  await RespondFromSaga(context, "Faulted On Reserve Delivery: " + string.Join("; ", context.Message.Exceptions.Select(x => x.Message)));
              })
              .Finalize(),

            When(ReserveDelivery.TimeoutExpired)
               .ThenAsync(async context =>
               {
                   await DiscardChanges(context);
                   await RespondFromSaga(context, "Timeout Expired On Reserve Delivery");
               })
               .Finalize());

        SetCompletedWhenFinalized();
    }

    private static async Task RespondFromSaga<T>(BehaviorContext<OrderStateInstance, T> context, string? error) where T : class
    {
        var endpoint = await context.GetSendEndpoint(context.Saga.ResponseAddress!);
        await endpoint.Send(new CreateOrderResponse
        {
            UserId = context.Saga.UserId,
            OrderId = context.Saga.OrderId,
            Price = context.Saga.Price,
            ErrorMessage = error
        }, r => r.RequestId = context.Saga.RequestId);
    }

    private static async Task DiscardChanges<T>(BehaviorContext<OrderStateInstance, T> context) where T : class
    {
        var messages = new List<object>
        {
            new DiscardPaymentChanges { UserId = context.Saga.UserId },
            new DiscardStockChanges { UserId = context.Saga.UserId },
            new DiscardDeliveryChanges { UserId = context.Saga.UserId }
        };
        await context.PublishBatch(messages);
    }
}

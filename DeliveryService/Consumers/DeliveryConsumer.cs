using Contracts.Saga;
using DeliveryService.Storage;
using MassTransit;

namespace DeliveryService.Consumers;

public class DeliveryConsumer : IConsumer<ReserveDeliveryRequest>
{
    public Task Consume(ConsumeContext<ReserveDeliveryRequest> context)
    {
        if (context.Message.ServiceError.GetValueOrDefault() == ServiceError.Delivery)
        {
            throw new InvalidOperationException("Error during creating a delivery");
        }

        DeliveryStorage.Store(context.Message.UserId, context.Message.OrderId, context.Message.DeliveryDate);
        return context.RespondAsync<ReserveDeliveryResponse>(new { context.Message.UserId, context.Message.OrderId, context.Message.DeliveryDate });
    }
}

using Contracts.Saga;
using DeliveryService.Storage;
using MassTransit;

namespace DeliveryService.Consumers;

public class DiscardChangesConsumer : IConsumer<DiscardDeliveryChanges>
{
    public Task Consume(ConsumeContext<DiscardDeliveryChanges> context)
    {
        DeliveryStorage.Remove(context.Message.UserId);
        return Task.CompletedTask;
    }
}

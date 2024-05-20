using System.Text.Json;
using Contracts.Saga;
using MassTransit;
using PaymentService.Storage;

namespace PaymentService.Consumers;

public class DiscardChangesConsumer(ILogger<DiscardChangesConsumer> logger) : IConsumer<DiscardPaymentChanges>
{
    public Task Consume(ConsumeContext<DiscardPaymentChanges> context)
    {
        logger.LogInformation($"Message is consumed by '{nameof(DiscardChangesConsumer)}': '{JsonSerializer.Serialize(context.Message)}");

        PaymentStorage.Remove(context.Message.UserId);
        return Task.CompletedTask;
    }
}

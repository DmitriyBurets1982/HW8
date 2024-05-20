using System.Text.Json;
using Contracts.Saga;
using MassTransit;
using PaymentService.Storage;

namespace PaymentService.Consumers;

public class PaymentConsumer(ILogger<PaymentConsumer> logger) : IConsumer<CreatePaymentRequest>
{
    public Task Consume(ConsumeContext<CreatePaymentRequest> context)
    {
        logger.LogInformation($"Message is consumed by '{nameof(PaymentConsumer)}': '{JsonSerializer.Serialize(context.Message)}");

        if (context.Message.ServiceError.GetValueOrDefault() == ServiceError.Payment)
        {
            throw new InvalidOperationException("Error during creating a payment");
        }

        PaymentStorage.Store(context.Message.UserId, context.Message.OrderId, context.Message.Price);
        return context.RespondAsync<CreatePaymentResponse>(new { context.Message.UserId, context.Message.OrderId, context.Message.Price });
    }
}

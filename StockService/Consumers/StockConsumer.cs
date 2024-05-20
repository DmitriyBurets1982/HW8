using Contracts.Saga;
using MassTransit;
using StockService.Storage;

namespace StockService.Consumers;

public class StockConsumer : IConsumer<ReserveStockRequest>
{
    public Task Consume(ConsumeContext<ReserveStockRequest> context)
    {
        if (context.Message.ServiceError.GetValueOrDefault() == ServiceError.Stock)
        {
            throw new InvalidOperationException("Error during creating a stock");
        }

        StockStorage.Store(context.Message.UserId, context.Message.OrderId);
        return context.RespondAsync<ReserveStockResponse>(new { context.Message.UserId, context.Message.OrderId });
    }
}

using Contracts.Saga;
using MassTransit;
using StockService.Storage;

namespace StockService.Consumers;

public class DiscardChangesConsumer : IConsumer<DiscardStockChanges>
{
    public Task Consume(ConsumeContext<DiscardStockChanges> context)
    {
        StockStorage.Remove(context.Message.UserId);
        return Task.CompletedTask;
    }
}

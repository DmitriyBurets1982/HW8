using System.Collections.Concurrent;
using StockService.Dtos;

namespace StockService.Storage;

internal static class StockStorage
{
    private static readonly ConcurrentDictionary<int, StockItem> _items = new ();

    public static StockItem? GetByUserId(int userId)
    {
        _items.TryGetValue(userId, out var item);
        return item;
    }

    public static void Store(int userId, int orderId)
    {
        var item = new StockItem { UserId = userId, OrderId = orderId };
        _items.AddOrUpdate(userId, item, (key, value) => item);
    }

    public static void Remove(int userId)
    {
        _items.Remove(userId, out _);
    }
}

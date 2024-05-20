using System.Collections.Concurrent;
using PaymentService.Models;

namespace PaymentService.Storage;

internal static class PaymentStorage
{
    private static readonly ConcurrentDictionary<int, PaymentItem> _items = new();

    public static PaymentItem? GetByUserId(int userId)
    {
        _items.TryGetValue(userId, out var item);
        return item;
    }

    public static void Store(int userId, int orderId, decimal price)
    {
        var item = new PaymentItem { UserId = userId, OrderId = orderId, Price = price };
        _items.AddOrUpdate(userId, item, (key, value) => item);
    }

    public static void Remove(int userId)
    {
        _items.Remove(userId, out _);
    }
}

using System.Collections.Concurrent;
using DeliveryService.Models;

namespace DeliveryService.Storage;

internal static class DeliveryStorage
{
    private static readonly ConcurrentDictionary<int, DeliveryItem> _items = new();

    public static DeliveryItem? GetByUserId(int userId)
    {
        _items.TryGetValue(userId, out var item);
        return item;
    }

    public static void Store(int userId, int orderId, DateTime deliveryDate)
    {
        var item = new DeliveryItem { UserId = userId, OrderId = orderId, DeliveryDate = deliveryDate };
        _items.AddOrUpdate(userId, item, (key, value) => item);
    }

    public static void Remove(int userId)
    {
        _items.Remove(userId, out _);
    }
}

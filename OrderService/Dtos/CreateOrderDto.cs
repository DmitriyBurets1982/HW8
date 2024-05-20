using Contracts.Saga;

namespace OrderService.Dtos;

public class CreateOrderDto
{
    public int UserId { get; set; }
    public int OrderId { get; set; }
    public decimal Price { get; set; }
    public ServiceError? ServiceError { get; set; }
}

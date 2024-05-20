using System.Text.Json;
using Contracts.Saga;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using OrderService.Dtos;

namespace OrderService.Controllers;

[ApiController]
[Route("orderservice/[controller]")]
public class OrderController(IBus bus, ILogger<OrderController> logger) : ControllerBase
{
    [HttpPost]
    public async Task<CreateOrderResponse> CreateOrder(CreateOrderDto dto)
    {
        logger.LogInformation("'{MethodName}' with parameter '{Dto}' was called", nameof(CreateOrder), JsonSerializer.Serialize(dto));

        var request = new CreateOrderRequest { UserId = dto.UserId, OrderId = dto.OrderId, Price = dto.Price, ServiceError = dto.ServiceError };
        var response = await bus.Request<CreateOrderRequest, CreateOrderResponse>(request);
        return response.Message;
    }
}

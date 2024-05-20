using DeliveryService.Storage;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryService.Controllers;

[ApiController]
[Route("deliveryservice/[controller]")]
public class DeliveryController(ILogger<DeliveryController> logger) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetItem([FromQuery] int userId)
    {
        logger.LogInformation("'{MethodName}' with parameter '{UserId}' was called", nameof(GetItem), userId);

        var item = DeliveryStorage.GetByUserId(userId);
        return item != null ? Ok(item) : NotFound();
    }
}

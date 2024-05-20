using Microsoft.AspNetCore.Mvc;

namespace DeliveryService.Controllers;

[ApiController]
[Route("deliveryservice/[controller]")]
public class HealthController(ILogger<HealthController> logger) : ControllerBase
{
    [HttpGet]
    public IActionResult GetHealth()
    {
        logger.LogInformation($"{nameof(DeliveryService)}->{nameof(GetHealth)}' was called");
        return Ok(new { status = $"{nameof(DeliveryService)}: OK" });
    }
}

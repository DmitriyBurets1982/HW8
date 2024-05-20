using Microsoft.AspNetCore.Mvc;

namespace StockService.Controllers
{
    [ApiController]
    [Route("stockservice/[controller]")]
    public class HealthController(ILogger<HealthController> logger) : ControllerBase
    {
        [HttpGet]
        public IActionResult GetHealth()
        {
            logger.LogInformation($"{nameof(StockService)}->{nameof(GetHealth)}' was called");
            return Ok(new { status = $"{nameof(StockService)}: OK" });
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace PaymentService.Controllers
{
    [ApiController]
    [Route("paymentservice/[controller]")]
    public class HealthController(ILogger<HealthController> logger) : ControllerBase
    {
        [HttpGet]
        public IActionResult GetHealth()
        {
            logger.LogInformation($"{nameof(PaymentService)}->{nameof(GetHealth)}' was called");
            return Ok(new { status = $"{nameof(PaymentService)}: OK" });
        }
    }
}

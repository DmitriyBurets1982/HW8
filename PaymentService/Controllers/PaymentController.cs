using Microsoft.AspNetCore.Mvc;
using PaymentService.Storage;

namespace PaymentService.Controllers
{
    [ApiController]
    [Route("paymentservice/[controller]")]
    public class PaymentController(ILogger<PaymentController> logger) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetItem([FromQuery] int userId)
        {
            logger.LogInformation("'{MethodName}' with parameter '{UserId}' was called", nameof(GetItem), userId);

            var item = PaymentStorage.GetByUserId(userId);
            return item != null ? Ok(item) : NotFound();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using StockService.Storage;

namespace StockService.Controllers
{
    [ApiController]
    [Route("stockservice/[controller]")]
    public class StockController(ILogger<StockController> logger) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetItem([FromQuery] int userId)
        {
            logger.LogInformation("'{MethodName}' with parameter '{UserId}' was called", nameof(GetItem), userId);

            var item = StockStorage.GetByUserId(userId);
            return item != null ? Ok(item) : NotFound();
        }
    }
}

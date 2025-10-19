using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(ILogger<OrdersController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateOrderAsync()
        {
            _logger.LogInformation("CreateOrder request received!");

            // Replace with code that does actual work
            await Task.CompletedTask;

            return Created((string?)null, Guid.NewGuid());
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using OrderService.Models.Requests;
using OrderService.Models.Responses;
using OrderService.Services;
using System.Net.Mime;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly IOrderProcessService _service;

        public OrdersController(ILogger<OrdersController> logger, IOrderProcessService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(CreateOrderResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateOrderAsync(CreateOrderRequest request)
        {
            _logger.LogInformation("CreateOrder request received. OrderId - {OrderId}.", request.OrderId);

            try
            {
                var result = await _service.CreateOrderAsync(request);
                if (result.Success == false)
                {
                    _logger.LogError("CreateOrder failed due to validation error. OrderId - {OrderId}.", request.OrderId);
                    return BadRequest($"Create order failed, reason: {result.FailedReason}.");
                }

                _logger.LogInformation("CreateOrder request processed successfully. OrderId - {OrderId}.", request.OrderId);
                return Created((string?)null, new CreateOrderResponse() { OrderId = request.OrderId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateOrder failed due to unexpected error. OrderId - {OrderId}.", request.OrderId);
                return StatusCode(StatusCodes.Status500InternalServerError , "Create order failed due to unexpeceted error. Please try again later.");
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using OrderService.Models.Requests;
using OrderService.Models.Responses;
using OrderService.Services;
using System.Net.Mime;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController(ILogger<OrdersController> logger, IOrderProcessService service) : ControllerBase
    {
        /// <summary>
        /// API to create order
        /// </summary>
        /// <param name="request">The request of body.</param>
        /// <response code="201">When order is successfully created. OrderId is included in the response.</response>
        /// <response code="400">When reqeust body contains incorrect values. An error message is returned to indicate the failed reason.</response>
        /// <response code="500">When unexpected error occurs.</response>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(CreateOrderResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateOrderAsync(CreateOrderRequest request)
        {
            logger.LogInformation("CreateOrder request received. OrderId - {OrderId}.", request.OrderId);

            try
            {
                var result = await service.CreateOrderAsync(request);
                if (result.Success == false)
                {
                    logger.LogError("CreateOrder failed due to validation error. OrderId - {OrderId}.", request.OrderId);
                    return BadRequest($"Create order failed, reason: {result.FailedReason}.");
                }

                logger.LogInformation("CreateOrder request processed successfully. OrderId - {OrderId}.", request.OrderId);

                // Best practice - create a GET API to get order info, then use CreatedAtAction instead.
                return Created((string?)null, new CreateOrderResponse() { OrderId = request.OrderId });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "CreateOrder failed due to unexpected error. OrderId - {OrderId}.", request.OrderId);
                return StatusCode(StatusCodes.Status500InternalServerError , "Create order failed due to unexpeceted error. Please try again later.");
            }
        }
    }
}

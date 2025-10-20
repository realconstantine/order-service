using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using OrderService.Controllers;
using OrderService.Models;
using OrderService.Models.Requests;
using OrderService.Models.Responses;
using OrderService.Services;

namespace Tests.Controllers
{
    public class OrdersControllerTests
    {
        private readonly OrdersController _controller;
        private readonly Mock<ILogger<OrdersController>> _loggerMock;
        private readonly Mock<IOrderProcessService> _serviceMock;

        public OrdersControllerTests()
        {
            _loggerMock = new Mock<ILogger<OrdersController>>();
            _serviceMock = new Mock<IOrderProcessService>();
            _controller = new OrdersController(_loggerMock.Object, _serviceMock.Object);
        }

        [Fact]
        public async Task GivenCreateOrderRequestWhenProcessResultIsASuccessThenReturnCreated()
        {
            var request = TestHelper.GetSampleRequest();

            _serviceMock.Setup(s => s.CreateOrderAsync(It.Is<CreateOrderRequest>(r => r.OrderId == request.OrderId)))
                .ReturnsAsync(
                    OrderProcessResult.NewResultFor(OrderProcessType.Create)
                    .WithOrderId(request.OrderId)
                    .WithSuccessFlag(true));

            IActionResult actionResult = await _controller.CreateOrderAsync(request);

            actionResult.Should().BeOfType<CreatedResult>();
            var underlyingResult = (ObjectResult)actionResult;

            var actualReturnValue = (CreateOrderResponse?)underlyingResult.Value;
            actualReturnValue?.OrderId.Should().Be(request.OrderId);
        }

        [Fact]
        public async Task GivenCreateOrderRequestWhenProcessResultIsAFailureThenReturnBadRequest()
        {
            var request = TestHelper.GetSampleRequest();

            _serviceMock.Setup(s => s.CreateOrderAsync(It.Is<CreateOrderRequest>(r => r.OrderId == request.OrderId)))
                .ReturnsAsync(
                    OrderProcessResult.NewResultFor(OrderProcessType.Create)
                    .WithOrderId(request.OrderId)
                    .WithSuccessFlag(false)
                    .WithFailedReason("Oops!"));

            IActionResult actionResult = await _controller.CreateOrderAsync(request);

            actionResult.Should().BeOfType<BadRequestObjectResult>();

            var underlyingResult = (ObjectResult)actionResult;
            underlyingResult.Value.Should().Be("Create order failed, reason: Oops!.");
        }

        [Fact]
        public async Task GivenCreateOrderRequestWhenProcessThrowsExceptionThenReturnInternalServerError()
        {
            var request = TestHelper.GetSampleRequest();

            _serviceMock.Setup(s => s.CreateOrderAsync(It.Is<CreateOrderRequest>(r => r.OrderId == request.OrderId)))
                .Throws<InvalidOperationException>();

            IActionResult actionResult = await _controller.CreateOrderAsync(request);

            var underlyingResult = (ObjectResult)actionResult;
            underlyingResult.Value.Should().Be("Create order failed due to unexpeceted error. Please try again later.");
            underlyingResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
    }   
}

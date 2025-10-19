using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using OrderService.Controllers;

namespace Tests.Controllers
{
    public class OrdersControllerTests
    {
        private readonly OrdersController _controller;
        private readonly Mock<ILogger<OrdersController>> _loggerMock;

        public OrdersControllerTests()
        {
            _loggerMock = new Mock<ILogger<OrdersController>>();
            _controller =  new OrdersController(_loggerMock.Object);
        }


        [Fact]
        public async Task GivenCreateOrderRequestWhenProcessResultIsASuccessThenReturnCreated()
        {
            var actionResult = await _controller.CreateOrderAsync();
            actionResult.Should().BeOfType<CreatedResult>();
        }
    }
}

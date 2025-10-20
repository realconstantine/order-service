using AutoMapper;
using FluentAssertions;
using Moq;
using OrderService.Data.Repositories;
using OrderService.Models.Requests;
using OrderService.Services;
using OrderService.Support;

namespace Tests.Services
{
    public class OrderProcessServiceTests
    {
        private readonly IOrderProcessService _service;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IOrderRepository> _repoMock;

        public OrderProcessServiceTests()
        {
            _repoMock = new Mock<IOrderRepository>();
            _mapperMock = new Mock<IMapper>();

            _service = new OrderProcessService(_repoMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GivenCreateOrderRequestWhenOrderNotCreatedThenCreateAndReturnSuccess()
        {
            var request = TestHelper.GetSampleRequest();
            _repoMock.Setup(r => r.GetOrderByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>()))
               .ReturnsAsync((OrderService.Data.Entities.Order?)null);

            _mapperMock.Setup(m => m.Map<OrderService.Data.Entities.Order>(It.Is<CreateOrderRequest>(r => r.OrderId == request.OrderId)))
                .Returns(new OrderService.Data.Entities.Order()
                {
                    OrderId = request.OrderId
                });

            var result = await _service.CreateOrderAsync(request);
            result.Success.Should().BeTrue();
            result.FailedReason.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task GivenCreateOrderRequestWhenOrderAlreadyCreatedThenReturnFailure()
        {
            var request = TestHelper.GetSampleRequest();
            _repoMock.Setup(r => r.GetOrderByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>()))
                .ReturnsAsync(new OrderService.Data.Entities.Order());

            var result = await _service.CreateOrderAsync(request);
            result.Success.Should().BeFalse();
            result.FailedReason.Should().Be(ErrorMessages.OrderAlreadyExists);
        }

        [Fact]
        public async Task GivenCreateOrderRequestWhenCustomerIsMissingThenReturnFailure()
        {
            var request = TestHelper.GetSampleRequest();
            request.CustomerName = string.Empty;

            _repoMock.Setup(r => r.GetOrderByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>()))
                .ReturnsAsync((OrderService.Data.Entities.Order?)null);

            var result = await _service.CreateOrderAsync(request);
            result.Success.Should().BeFalse();
            result.FailedReason.Should().Be(ErrorMessages.MissingCustomerName);
        }

        [Fact]
        public async Task GivenCreateOrderRequestWhenItemListIsEmptyThenReturnFailure()
        {
            var request = TestHelper.GetSampleRequest();
            request.Items = [];

            _repoMock.Setup(r => r.GetOrderByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>()))
                .ReturnsAsync((OrderService.Data.Entities.Order?)null);

            var result = await _service.CreateOrderAsync(request);
            result.Success.Should().BeFalse();
            result.FailedReason.Should().Be(ErrorMessages.EmptyItems);
        }

        [Fact]
        public async Task GivenCreateOrderRequestWhenQuantityIsIncorrectThenReturnFailure()
        {
            var request = TestHelper.GetSampleRequest();
            request.Items.First().Quantity = 0;

            _repoMock.Setup(r => r.GetOrderByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>()))
                .ReturnsAsync((OrderService.Data.Entities.Order?)null);

            var result = await _service.CreateOrderAsync(request);
            result.Success.Should().BeFalse();
            result.FailedReason.Should().Be(ErrorMessages.IncorrectQuantityValues);
        }

        [Fact]
        public async Task GivenCreateOrderRequestWhenItemHasDupesThenReturnFailure()
        {
            var request = TestHelper.GetSampleRequest();
            var productId = Guid.NewGuid();
            request.Items = [
                new OrderedItem() { ProductId = productId, Quantity = 1 },
                new OrderedItem() { ProductId = productId, Quantity = 2 }
            ];

            _repoMock.Setup(r => r.GetOrderByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>()))
                .ReturnsAsync((OrderService.Data.Entities.Order?)null);

            var result = await _service.CreateOrderAsync(request);
            result.Success.Should().BeFalse();
            result.FailedReason.Should().Be(ErrorMessages.DuplicateProducts);
        }
    }
}

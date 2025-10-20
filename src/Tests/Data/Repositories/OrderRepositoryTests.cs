using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using OrderService.Data;
using OrderService.Data.Repositories;

namespace Tests.Data.Repositories
{
    public class OrderRepositoryTests
    {
        private readonly IOrderRepository _repo;
        private readonly Mock<IDbContextFactory<OrderServiceDataContext>> _dbContextFactoryMock;

        public OrderRepositoryTests() 
        {
            _dbContextFactoryMock = new Mock<IDbContextFactory<OrderServiceDataContext>>();
            _repo = new OrderRepository(_dbContextFactoryMock.Object);
        }

        [Fact]
        public async Task GivenOrderIdWhenIncludeProductsThenQueryResult()
        {
            var context = TestHelper.CreateInMemorySqliteDbAndReturnActiveDbContext();
            var expectedOrder = TestHelper.GetSampleOrderEntity();
            await context.AddAsync(expectedOrder);
            await context.SaveChangesAsync();

            _dbContextFactoryMock.Setup(factory => factory.CreateDbContext())
                .Returns(context);

            var acutalOrder = await _repo.GetOrderByIdAsync(expectedOrder.OrderId, true);
            acutalOrder?.OrderId.Should().Be(expectedOrder.OrderId);
            acutalOrder?.Products.Should().ContainSingle(p => p.ProductId == expectedOrder.Products[0].ProductId);
        }

        [Fact]
        public async Task GivenOrderThenInsertResult()
        {
            var context = TestHelper.CreateInMemorySqliteDbAndReturnActiveDbContext();
            var expectedOrder = TestHelper.GetSampleOrderEntity();
            _dbContextFactoryMock.Setup(factory => factory.CreateDbContext())
                .Returns(context);

            var orderId = await _repo.CreateOrderAsync(expectedOrder);
            orderId.Should().Be(expectedOrder.OrderId);
        }
    }
}

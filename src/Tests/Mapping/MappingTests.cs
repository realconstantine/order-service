using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using OrderService.Data.Entities;
using OrderService.Mapping;

namespace Tests.Mapping
{
    public class MappingTests
    {
        private readonly IMapper _mapper;

        public MappingTests()
        {
            var logFactory = LoggerFactory.Create(config => config.AddConsole());
            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile<OrderServiceProfile>(), logFactory);

            _mapper = mapperConfiguration.CreateMapper();
        }

        [Fact]
        public void GivenRequestThenMapToEntity()
        {
            var request = TestHelper.GetSampleRequest();
            var entity = _mapper.Map<Order>(request);

            entity.CustomerName.Should().Be(request.CustomerName);
            entity.OrderId.Should().Be(request.OrderId);
            entity.CreatedAt.Should().Be(request.CreatedAt);
            entity.ModifyAt.Should().Be(request.CreatedAt);
            entity.ModifyBy.Should().Be(Environment.MachineName);
            entity.Products.Should().HaveCount(1);
            var product = entity.Products.Single();
            product.OrderId.Should().Be(request.OrderId);
            product.Order.Should().BeEquivalentTo(entity);
            product.ProductId.Should().Be(request.Items.First().ProductId);
            product.Quantity.Should().Be(request.Items.First().Quantity);
        }
    }
}

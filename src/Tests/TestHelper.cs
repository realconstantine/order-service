using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Data.Entities;
using OrderService.Models.Requests;

namespace Tests
{
    internal static class TestHelper
    {
        internal static CreateOrderRequest GetSampleRequest() =>
            new CreateOrderRequest()
            {
                OrderId = Guid.NewGuid(),
                CustomerName = "John Doe",
                CreatedAt = DateTime.Now,
                Items = [new OrderedItem() { ProductId = Guid.NewGuid(), Quantity = 1 }]
            };

        internal static Order GetSampleOrderEntity()
        {
            var orderId = Guid.NewGuid();
            var order = new Order
            {
                OrderId = orderId,
                CustomerName = "John Doe",
                CreatedAt = DateTime.Now,
                ModifyAt = DateTime.Now,
                ModifyBy = "Test"
            };

            var product = new OrderedProduct()
            {
                OrderId = orderId,
                ProductId = Guid.NewGuid(),
                Quantity = 99,
                CreatedAt = DateTime.Now,
                ModifyAt = DateTime.Now,
                ModifyBy = "Test",
                Order = order
            };

            order.Products.Add(product);

            return order;
        }

        internal static OrderServiceDataContext CreateInMemorySqliteDbAndReturnActiveDbContext()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();

            var contextOptions = new DbContextOptionsBuilder<OrderServiceDataContext>()
                .UseSqlite(connection)
                .Options;

            var context = new OrderServiceDataContext(contextOptions);
            context.Database.EnsureCreated();

            return context;
        }
    }
}

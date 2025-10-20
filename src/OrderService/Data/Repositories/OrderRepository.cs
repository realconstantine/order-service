using Microsoft.EntityFrameworkCore;
using OrderService.Data.Entities;

namespace OrderService.Data.Repositories
{
    public interface IOrderRepository
    {
        Task<Order?> GetOrderByIdAsync(Guid orderId, bool includeItemList = false);
        Task<Guid> CreateOrderAsync(Order order);
    }

    public class OrderRepository : IOrderRepository
    {
        private readonly IDbContextFactory<OrderServiceDataContext> _contextFactory;

        public OrderRepository(IDbContextFactory<OrderServiceDataContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Order?> GetOrderByIdAsync(Guid orderId, bool includeItemList = false)
        {
            using var context = _contextFactory.CreateDbContext();

            if (includeItemList)
            {
                return await context.Orders.Include(order => order.Products)
                    .FirstOrDefaultAsync(order => order.OrderId == orderId);
            }

            return await context.Orders.FirstOrDefaultAsync(order => order.OrderId == orderId);
        }

        public async Task<Guid> CreateOrderAsync(Order order)
        {
            using var context = _contextFactory.CreateDbContext();
            await context.Orders.AddAsync(order);
            await context.SaveChangesAsync();

            return order.OrderId;
        }
    }
}

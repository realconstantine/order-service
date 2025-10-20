using Microsoft.EntityFrameworkCore;
using OrderService.Data.Entities;

namespace OrderService.Data
{
    public class OrderServiceDataContext : DbContext
    {
        public OrderServiceDataContext(DbContextOptions<OrderServiceDataContext> options)
            : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderedProduct> OrderedProducts { get; set; }
    }
}

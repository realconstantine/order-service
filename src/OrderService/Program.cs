using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Data.Repositories;
using OrderService.Services;

namespace OrderService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            var orderConnnectionString = GetOrderDbConnectionString(builder.Configuration);
            builder.Services.AddDbContextFactory<OrderServiceDataContext>(options =>
            {
                options.UseSqlite(orderConnnectionString);
            });

            builder.Services.AddSingleton<IOrderRepository, OrderRepository>();
            builder.Services.AddSingleton<IOrderProcessService, OrderProcessService>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        private static string? GetOrderDbConnectionString(IConfiguration configuration) =>
             configuration.GetConnectionString("OrdersSqliteDb") ?? "FileName=./orders.db";

    }
}

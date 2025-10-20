using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using OrderService.Data;
using OrderService.Data.Repositories;
using OrderService.Mapping;
using OrderService.Services;
using System.Reflection;

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

            builder.Services.AddAutoMapper(config =>
            {
                config.AddProfile<OrderServiceProfile>();
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(setup =>
            {
                setup.SwaggerDoc("v1", new OpenApiInfo 
                { 
                    Title = "Ordcer Service API", Version = "v1" 
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                setup.IncludeXmlComments(xmlPath);
            });

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

using OrderService.Data.Entities;
using OrderService.Data.Repositories;
using OrderService.Models;
using OrderService.Models.Requests;

namespace OrderService.Services
{
    public interface IOrderProcessService
    {
        Task<OrderProcessResult> CreateOrderAsync(CreateOrderRequest request);
    }

    public class OrderProcessService : IOrderProcessService
    {
        private readonly IOrderRepository _repo;

        public OrderProcessService(IOrderRepository repo)
        {
            _repo = repo;
        }

        public async Task<OrderProcessResult> CreateOrderAsync(CreateOrderRequest request)
        {
            var validationResult = await ValidateOrderCreationStatus(request);
            if (validationResult != null)
            {
                return validationResult;
            }

            var orderEntity = BuildEntity(request);
            await _repo.CreateOrderAsync(orderEntity);

            return GetSucceedOrderProcessResult(OrderProcessType.Create, request.OrderId);
        }

        private Order BuildEntity(CreateOrderRequest request)
        {
            var orderEntity = new Order()
            {
                OrderId = request.OrderId,
                CustomerName = request.CustomerName,
                CreatedAt = request.CreatedAt
            };

            var productEntites = request.Items.Select(item => new OrderedProduct()
            {
                OrderId = request.OrderId,
                Order = orderEntity,
                ProductId = item.ProductId,
                Quantity = item.Quantity
            }).ToList();

            orderEntity.Products.AddRange(productEntites);

            return orderEntity;
        }

        private async Task<OrderProcessResult?> ValidateOrderCreationStatus(CreateOrderRequest request)
        {
            var order = await _repo.GetOrderByIdAsync(request.OrderId);
            if (order != null)
            {
                return GetFailedOrderProcessResult(OrderProcessType.Create, "Order with same Id is already created");
            }

            if (string.IsNullOrWhiteSpace(request.CustomerName))
            {
                return GetFailedOrderProcessResult(OrderProcessType.Create, "Customer Name must be present");
            }

            var items = request.Items;
            if (items.Count == 0)
            {
                return GetFailedOrderProcessResult(OrderProcessType.Create, "No items found in the order");
            }

            if (items.Any(item => item.Quantity <= 0))
            {
                return GetFailedOrderProcessResult(OrderProcessType.Create, "There are items with incorrect Quantity values");
            }

            return null;
        }

        private static OrderProcessResult GetSucceedOrderProcessResult(OrderProcessType orderProcessType, Guid orderId) =>
            new(true, orderProcessType, OrderId: orderId);

        private static OrderProcessResult GetFailedOrderProcessResult(OrderProcessType orderProcessType, string failedReason) =>
            new(false, orderProcessType, FailedReason: failedReason);
    }
}

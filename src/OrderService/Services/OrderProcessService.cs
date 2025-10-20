using AutoMapper;
using OrderService.Data.Entities;
using OrderService.Data.Repositories;
using OrderService.Models;
using OrderService.Models.Requests;
using OrderService.Support;

namespace OrderService.Services
{
    public interface IOrderProcessService
    {
        Task<OrderProcessResult> CreateOrderAsync(CreateOrderRequest request);
    }

    public class OrderProcessService(IOrderRepository repo, IMapper mapper) : IOrderProcessService
    {
        public async Task<OrderProcessResult> CreateOrderAsync(CreateOrderRequest request)
        {
            var result = OrderProcessResult.NewResultFor(OrderProcessType.Create)
                .WithOrderId(request.OrderId);

            // Validate if we are okay to proceed with order creation
            var (validationPass, failedReason) = await ValidateBeforeOrderCreationAsync(request);
            if (!validationPass)
            {
                return result.WithSuccessFlag(false)
                    .WithFailedReason(failedReason);
            }

            // Map request to entity
            var orderEntity = BuildOrderEntity(request);

            // Create order
            await repo.CreateOrderAsync(orderEntity);

            return result.WithSuccessFlag(true);
        }

        private Order BuildOrderEntity(CreateOrderRequest request) =>
            mapper.Map<Order>(request);


        private async Task<(bool ValidationPass, string? FailedReason)> ValidateBeforeOrderCreationAsync(CreateOrderRequest request)
        {
            var order = await repo.GetOrderByIdAsync(request.OrderId);
            if (order != null)
            {
                return (false, ErrorMessages.OrderAlreadyExists);
            }

            if (string.IsNullOrWhiteSpace(request.CustomerName))
            {
                return (false, ErrorMessages.MissingCustomerName);
            }

            var items = request.Items;
            if (items.Count == 0)
            {
                return (false, ErrorMessages.EmptyItems);
            }

            if (items.Any(item => item.Quantity <= 0))
            {
                return (false, ErrorMessages.IncorrectQuantityValues);
            }

            if (items.GroupBy(item => item.ProductId)
                .Any(group => group.Count() > 1))
            {
                return (false, ErrorMessages.DuplicateProducts);
            }

            return (true, null);
        }
    }
}

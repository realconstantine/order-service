namespace OrderService.Models.Requests
{
    public class CreateOrderRequest
    {
        public required Guid OrderId { get; set; }
        public required string CustomerName { get; set; }
        public required ICollection<OrderedItems> Items { get; set; }
        public required DateTime CreatedAt { get;set; }
    }

    public class OrderedItems
    {
        public required Guid ProductId { get; set; }
        public required int Quantity { get; set; }
    }
}

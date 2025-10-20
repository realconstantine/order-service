namespace OrderService.Models.Requests
{
    public class CreateOrderRequest
    {
        /// <summary>
        /// The Order Id
        /// </summary>
        public required Guid OrderId { get; set; }
        /// <summary>
        /// Customer's Name
        /// </summary>
        public required string CustomerName { get; set; }
        /// <summary>
        /// Items assoicated to the order
        /// </summary>
        public required ICollection<OrderedItems> Items { get; set; }
        /// <summary>
        /// Date time when order is created
        /// </summary>
        public required DateTime CreatedAt { get;set; }
    }

    public class OrderedItems
    {
        /// <summary>
        /// The Product Id
        /// </summary>
        public required Guid ProductId { get; set; }
        /// <summary>
        /// Quantity of the ordered product
        /// </summary>
        public required int Quantity { get; set; }
    }
}

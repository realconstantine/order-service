namespace OrderService.Models.Responses
{
    public class CreateOrderResponse
    {
        /// <summary>
        /// The Order Id
        /// </summary>
        public required Guid OrderId { get; set; }
    }
}
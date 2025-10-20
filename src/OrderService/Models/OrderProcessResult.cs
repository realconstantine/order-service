namespace OrderService.Models
{
    public class OrderProcessResult
    {
        public bool Success { get; private set; }
        public OrderProcessType OrderProcessType { get; private set; }
        public Guid? OrderId { get; private set; }
        public string? FailedReason { get; private set; }

        private OrderProcessResult() { }

        public static OrderProcessResult NewResultFor(OrderProcessType orderProcessType) => new()
        {
            OrderProcessType = orderProcessType
        };

        public OrderProcessResult WithSuccessFlag(bool isSuccess)
        {
            Success = isSuccess;
            return this;
        }

        public OrderProcessResult WithFailedReason(string? failedReason)
        {
            FailedReason = failedReason;
            return this;
        }

        public OrderProcessResult WithOrderId(Guid orderId)
        {
            OrderId = orderId;
            return this;
        }
    };
}

namespace OrderService.Models
{
    public record OrderProcessResult (bool Success, OrderProcessType OrderProcessType, Guid? OrderId = null, string? FailedReason = null);
}

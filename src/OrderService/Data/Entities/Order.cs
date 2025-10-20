using System.ComponentModel.DataAnnotations;

namespace OrderService.Data.Entities
{
    public class Order
    {
        [Key]
        public Guid OrderId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public List<OrderedProduct> Products { get; set; } = [];
        public DateTime CreatedAt { get; set; }
        public DateTime ModifyAt { get; set; }
        public string ModifyBy { get; set; } = string.Empty;
    }
}

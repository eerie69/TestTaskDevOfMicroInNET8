using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using server.Constants;


namespace server.Models
{
    [Table("Order")]
    public class Order
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public OrderStatus Status { get; set; } 
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }
        [Required]
        public string Address { get; set; } = string.Empty;
        [Required]
        public string PaymentMethod { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
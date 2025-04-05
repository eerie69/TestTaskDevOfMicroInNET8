using server.Models;
using System.ComponentModel.DataAnnotations;

namespace server.Dtos.Order
{
    public class OrderItemRequestDto
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

    }
}
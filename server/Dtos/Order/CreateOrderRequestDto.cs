using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using server.Dtos.Product;

namespace server.Dtos.Order
{
    public class CreateOrderRequestDto
    {
        [Required]
        public string Address { get; set; } = string.Empty;

        [Required]
        public string PaymentMethod { get; set; } = string.Empty;

        [Required]
        public List<OrderItemRequestDto> OrderItems { get; set; } = new();

    }
}
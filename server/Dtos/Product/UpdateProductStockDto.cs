using System.ComponentModel.DataAnnotations;

namespace server.Dtos.Product
{
    public class UpdateProductStockDto
    {
        [Required]
        public int Quantity { get; set; }
    }
}
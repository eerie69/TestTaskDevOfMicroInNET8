using server.Dtos.Product;
using server.Models;

namespace server.Mappers
{
    public static class ProductMappers
    {
        public static ProductDto ToProductDto(this Product productModel)
        {
            return new ProductDto
            {
                Id = productModel.Id,
                Name = productModel.Name,
                Description = productModel.Description,
                Price = productModel.Price,
                Quantity = productModel.Stock
            };
        }

        public static Product ToProductFromCreateDTO(this CreateProductDto productModel)
        {

            return new Product
            {
                Name = productModel.Name,
                Description = productModel.Description,
                Price = productModel.Price,
                Stock = productModel.Stock
            };
        }
    }
}
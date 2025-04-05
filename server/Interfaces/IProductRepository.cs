using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Dtos.Product;
using server.Helpers;
using server.Models;

namespace server.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync(QueryObject query);
        Task<Product?> GetByIdAsync(int id);
        Task<Product> CreateAsync(Product productModel);
        Task<Product?> UpdateAsync(int id, UpdateProductDto productDto);
        Task<Product?> UpdateStockAsync(int id, UpdateProductStockDto productDto);
        Task<Product?> DeleteAsync(int id);
    }
}
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Dtos.Product;
using server.Helpers;
using server.Interfaces;
using server.Models;

namespace server.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDBContext _context;
        public ProductRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<Product> CreateAsync(Product productModel)
        {
            await _context.Products.AddAsync(productModel);
            await _context.SaveChangesAsync();
            return productModel;
        }

        public async Task<Product?> DeleteAsync(int id)
        {
            var productModel = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (productModel is null) return null;

            _context.Products.Remove(productModel);
            await _context.SaveChangesAsync();
            return (productModel);
        }

        public async Task<List<Product>> GetAllAsync(QueryObject query)
        {
            var products = _context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                string titleLower = query.Name.ToLower();
                products = products.Where(s => s.Name.ToLower().Contains(query.Name));
            }

            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if (query.SortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    products = query.IsDescending ? products.OrderByDescending(s => s.Name) : products.OrderBy(s => s.Name);
                }
            }

            int pageSize = query.PageSize > 0 ? query.PageSize : 10;
            int pageNumber = query.PageNumber > 0 ? query.PageNumber : 1;

            var skipNumber = (pageNumber - 1) * pageSize;

            return await products.Skip(skipNumber).Take(pageSize).ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Product?> UpdateAsync(int id, UpdateProductDto productDto)
        {
            var existingProduct = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (existingProduct is null) return null;

            existingProduct.Name = productDto.Name;
            existingProduct.Description = productDto.Description;
            existingProduct.Price = productDto.Price;

            await _context.SaveChangesAsync();
            return existingProduct;
        }

        public async Task<Product?> UpdateStockAsync(int id, UpdateProductStockDto productDto)
        {
            var existingStock = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (existingStock is null) return null;

            var newStock = existingStock.Stock + productDto.Quantity;
            if (newStock < 0)
                throw new InvalidOperationException("Not enough stock.");

            existingStock.Stock = newStock;
            await _context.SaveChangesAsync();
            return existingStock;
        }
    }
}
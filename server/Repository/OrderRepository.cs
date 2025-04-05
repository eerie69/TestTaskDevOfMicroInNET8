using server.Service;
using Microsoft.EntityFrameworkCore;
using server.Constants;
using server.Data;
using server.Dtos.Order;
using server.Dtos.Product;
using server.Interfaces;
using server.Mappers;
using server.Models;
using Humanizer;

namespace server.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly ProductHttpClientService _productService;
        public OrderRepository(ApplicationDBContext context, ProductHttpClientService productService)
        {
            _context = context;
            _productService = productService;
        }
        public async Task<Order> CreateOrderAsync(string userId, CreateOrderRequestDto orderDto)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                decimal totalPrice = 0;
                var orderItems = new List<OrderItem>();

                foreach (var item in orderDto.OrderItems)
                {
                    var product = await _productService.GetProductByIdAsync(item.ProductId);
                    if (product == null)
                        throw new InvalidOperationException($"Товар с ID {item.ProductId} не найден");

                    if (product.Quantity < item.Quantity)
                        throw new InvalidOperationException($"Недостаточно товара (ID {item.ProductId}) на складе");

                    var reserved = await _productService.ReserveProductStockAsync(item.ProductId, item.Quantity);
                    if (!reserved)
                        throw new InvalidOperationException($"Не удалось зарезервировать товар (ID {item.ProductId})");

                    totalPrice += product.Price * item.Quantity;

                    orderItems.Add(new OrderItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = product.Price
                    });
                }

                // Создаём заказ
                var order = new Order
                {
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    Status = OrderStatus.Pending,
                    TotalPrice = totalPrice,
                    Address = orderDto.Address,
                    PaymentMethod = orderDto.PaymentMethod,
                    Items = orderItems
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return order;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при создании заказа: {ex.Message}");
                await transaction.RollbackAsync();
                throw;
            }

        }
    

        public async Task<List<Order>> GetUserOrdersAsync(string userId)
        {
            var orders = await _context.Orders
                    .Include(c => c.Items)
                    .Where(a => a.UserId == userId && !a.IsDeleted)
                    .ToListAsync();
            
            return orders;
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Orders.Include(e => e.Items).FirstOrDefaultAsync(i => i.Id == id);
        }
    }
}
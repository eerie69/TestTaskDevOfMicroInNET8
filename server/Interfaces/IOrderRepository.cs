using server.Dtos.Order;
using server.Models;

namespace server.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrderAsync(string userId, CreateOrderRequestDto orderDto);
        Task<Order?> GetByIdAsync(int id);
        Task<List<Order>> GetUserOrdersAsync(string userId);
        
    }
}
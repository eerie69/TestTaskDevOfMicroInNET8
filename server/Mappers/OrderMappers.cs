using server.Constants;
using server.Dtos.Order;
using server.Models;

namespace server.Mappers
{
    public static class OrderMappers
    {
        public static OrderDto ToOrderUserDto(this Order orderModel)
        {
            return new OrderDto
            {
                Id = orderModel.Id,
                UserId = orderModel.UserId,
                CreatedAt = orderModel.CreatedAt,
                Status = orderModel.Status,
                TotalPrice = orderModel.TotalPrice,
                Address = orderModel.Address,
                PaymentMethod = orderModel.PaymentMethod,
                Items = orderModel.Items.Select(s => s.ToOrderItemsDto()).ToList()
            };
        }

        public static OrderItemDto ToOrderItemsDto(this OrderItem orderModel)
        {
            return new OrderItemDto
            {
                Id = orderModel.Id,
                ProductId = orderModel.ProductId,
                Quantity = orderModel.Quantity,
                UnitPrice = orderModel.UnitPrice,
            };


        }
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using server.Dtos.Order;
using server.Interfaces;
using server.Mappers;
using server.Models;
using server.Service;

namespace server.Controllers
{
    [ApiController]
    [Route("server/Order")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepo;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<OrderController> _logger;
        public OrderController(IOrderRepository orderRepo, UserManager<AppUser> userManager,
            ILogger<OrderController> logger)
        {
            _orderRepo = orderRepo;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var order = await _orderRepo.GetByIdAsync(id);
            if(order == null) return NotFound();
            
            return Ok(order.ToOrderUserDto());
        }

        [Authorize]
        [HttpPost("create-orders")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequestDto orderModel)
        {
            try
            {
                var userId = User.GetUserId();
                if(userId == null) return NotFound();

                var order = await _orderRepo.CreateOrderAsync(userId, orderModel);
                return CreatedAtAction(nameof(GetById), new { id = order.Id }, order.ToOrderUserDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order: {Message}", ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize]
        [HttpGet("user-orders")]
        public async Task<IActionResult> UserOrders()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.GetUserId();
            if(userId == null) return NotFound();

            var orders = await _orderRepo.GetUserOrdersAsync(userId);
            if(orders == null) return NotFound();

            var ordersDto = orders.Select(s => s.ToOrderUserDto()).ToList();
            return Ok(ordersDto);
        }
    }
}
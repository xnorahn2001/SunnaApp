using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnaaPlatform.Api.Models;
using SnaaPlatform.Api.Services;
using System.Security.Claims;

namespace SnaaPlatform.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly ISupabaseService _supabaseService;

        public OrderController(ISupabaseService supabaseService)
        {
            _supabaseService = supabaseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var orders = await _supabaseService.GetOrdersAsync(userId);
            return Ok(orders);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            order.ClientId = userId;
            order.CreatedAt = DateTime.UtcNow;

            var success = await _supabaseService.CreateOrderAsync(order);
            if (!success) return StatusCode(500, new { message = "Error creating order" });

            return Ok(order);
        }
    }
}

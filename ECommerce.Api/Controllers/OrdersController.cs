using ECommerce.Api.Extentions;
using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Application.Police;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IServicesOrder servicesOrder;
        private readonly IAuthorizationService authorizationService;

        public OrdersController(IServicesOrder servicesOrder,IAuthorizationService authorizationService)
        {
            this.servicesOrder = servicesOrder;
            this.authorizationService = authorizationService;
        }
        [Authorize(Roles ="Customer")]
        [HttpPost("Checkout")]
        public async Task<IActionResult> CheckOut(CheckOutDto dto)
        {
            var userid = User.GetUserid();
            
            var checkout = servicesOrder.Checkout(userid, dto);
            if (!checkout)
            {
                return BadRequest();
            }
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetOrders(string userid)
        {
            var orders = servicesOrder.GetOrders(userid);
            if (orders is null)
            {
                return NotFound();
            }
            return Ok(orders);
        }

        [Authorize(Roles = "Customer")]
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var userid = User.GetUserid();
            var orders = servicesOrder.GetOrders(userid);
            if (orders is null)
            {
                return NotFound();
            }
            return Ok(orders);
        }
       
        [HttpGet("{OrderId}")]
        public async Task<IActionResult> GetOrder(int OrderId)
        {
            var order = servicesOrder.GetEntityById(OrderId);
            if (order is null)
            {
                return NotFound();
            }
            var result = await authorizationService.AuthorizeAsync(User, order, new AccessUserOnHisOrderRequirement());
            if (!result.Succeeded)
            {
                return Forbid();
            }
            var orderbyid = servicesOrder.GetOrderById(OrderId);

            return Ok(orderbyid);
        }
    }
}

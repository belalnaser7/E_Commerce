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
        public async Task<IActionResult> CheckOutAsync(CheckOutDto dto)
        {
            var userid = User.GetUserid();
            
            var result =await servicesOrder.CheckoutAsync(userid, dto);
            return result.ToActionResult();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("user/{userid}")]
        public async Task<IActionResult> GetOrders(string userid)
        {
            var result =await servicesOrder.GetOrdersAsync(userid);
            return result.ToActionResult();
        }

        [Authorize(Roles = "Customer")]
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var userid = User.GetUserid();
            var result =await servicesOrder.GetOrdersAsync(userid);
            return result.ToActionResult();
        }
       
        [HttpGet("{OrderId}")]
        public async Task<IActionResult> GetOrder(int OrderId)
        {
            var order =await servicesOrder.GetEntityByIdAsync(OrderId);
            if (!order.IsSuccess)
            {
                return order.ToActionResult();
            }
            var result = await authorizationService.AuthorizeAsync(User, order.Data, new AccessUserOnHisOrderRequirement());
            if (!result.Succeeded)
            {
                return Forbid();
            }
            var result1 =await servicesOrder.GetOrderByIdAsync(OrderId);

            return result1.ToActionResult();
        }
    }
}

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
    public class CartController : ControllerBase
    {
        private readonly IServicesCart servicesCart;
        private readonly IAuthorizationService authorizationService;

        public CartController(IServicesCart servicesCart, IAuthorizationService authorizationService)
        {
            this.servicesCart = servicesCart;
            this.authorizationService = authorizationService;
        }
        //For Admins 
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCart(string userId)
        {
            var cart = servicesCart.GetByUserId(userId);
            if (!cart.IsSuccess)
            {
              return cart.ToActionResult() ;
            }
            var result = await authorizationService.AuthorizeAsync(
                User,
                cart.Data,
                new CanViewCartRequirement());

            if (!result.Succeeded)
            {
                return Forbid();
            }
            var result1 = servicesCart.GetCart(userId);
            return result1.ToActionResult();
        }
        //for Customer 
        [Authorize(Roles = "Customer")]
        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userid = User.GetUserid();

            var cartitems = servicesCart.GetCart(userid);
            return cartitems.ToActionResult();
        }
        //For Admins 
        [HttpDelete("{cartitemid}/{userid}")]
        public async Task<IActionResult> DeleteCart(int cartitemid, string userid)
        {
            var cart = servicesCart.GetByUserId(userid);
            if (!cart.IsSuccess)
            {
                return cart.ToActionResult();
            }
            var result = await authorizationService.AuthorizeAsync(User, cart.Data, new CanViewCartRequirement());
            if (!result.Succeeded)
            {
                return Forbid();
            }
            var result1 = servicesCart.RemoveItem(userid, cartitemid);
            return result1.ToActionResult();
        }
        //for Customer 
        [Authorize(Roles = "Customer")]
        [HttpDelete("{cartitemid}")]
        public async Task<IActionResult> DeleteCart(int cartitemid)
        {   
            var userid = User.GetUserid();

            var result = servicesCart.RemoveItem(userid, cartitemid);
            return result.ToActionResult();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("clearCart/{userid}")]
        public async Task<IActionResult> ClearCart(string userid)
        {
            var result = servicesCart.ClearCart(userid);
            return result.ToActionResult();
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> ClearCart()
        {
            var userid = User.GetUserid();
            var result = servicesCart.ClearCart(userid);
            return result.ToActionResult();
        }

        [Authorize(Roles ="Customer")]
        [HttpPost]
        public async Task<IActionResult> AddToCart( AddToCartDto dto)
        {
            var userid = User.GetUserid();
            
            var result = servicesCart.AddToCart(userid, dto);
            return result.ToActionResult();
        }
        [Authorize(Roles = "Customer")]
        [HttpPut]
        public async Task<IActionResult> Update(UpdateCartItemDto dto)
        {
            var userid = User.GetUserid();
            var result = servicesCart.UpdateQuantity(userid, dto);
            return result.ToActionResult(); ;
        }
    }
}

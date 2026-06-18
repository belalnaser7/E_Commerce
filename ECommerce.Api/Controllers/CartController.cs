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
        public async Task<IActionResult> GetCartAsync(string userId)
        {
            var cart =await servicesCart.GetByUserIdAsync(userId);
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
            var result1 = await servicesCart.GetCartAsync(userId);
            return result1.ToActionResult();
        }
        //for Customer 
        [Authorize(Roles = "Customer")]
        [HttpGet]
        public async Task<IActionResult> GetCartAsync()
        {
            var userid = User.GetUserid();

            var cartitems = await servicesCart.GetCartAsync(userid);
            return cartitems.ToActionResult();
        }
        //For Admins 
        [HttpDelete("{cartitemid}/{userid}")]
        public async Task<IActionResult> DeleteCartAsync(int cartitemid, string userid)
        {
            var cart = await servicesCart.GetByUserIdAsync(userid);
            if (!cart.IsSuccess)
            {
                return cart.ToActionResult();
            }
            var result = await authorizationService.AuthorizeAsync(User, cart.Data, new CanViewCartRequirement());
            if (!result.Succeeded)
            {
                return Forbid();
            }
            var result1 =await servicesCart.RemoveItemAsync(userid, cartitemid);
            return result1.ToActionResult();
        }
        //for Customer 
        [Authorize(Roles = "Customer")]
        [HttpDelete("{cartitemid}")]
        public async Task<IActionResult> DeleteCartAsync(int cartitemid)
        {   
            var userid = User.GetUserid();
            var result = await servicesCart.RemoveItemAsync(userid, cartitemid);
            return result.ToActionResult();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("clearCart/{userid}")]
        public async Task<IActionResult> ClearCartAsync(string userid)
        {
            var result =await servicesCart.ClearCartAsync(userid);
            return result.ToActionResult();
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> ClearCartAsync()
        {
            var userid = User.GetUserid();
            var result =await servicesCart.ClearCartAsync(userid);
            return result.ToActionResult();
        }

        [Authorize(Roles ="Customer")]
        [HttpPost]
        public async Task<IActionResult> AddToCartAsync( AddToCartDto dto)
        {
            var userid = User.GetUserid();
            
            var result =await servicesCart.AddToCartAsync(userid, dto);
            return result.ToActionResult();
        }
        [Authorize(Roles = "Customer")]
        [HttpPut]
        public async Task<IActionResult> UpdateAsync(UpdateCartItemDto dto)
        {
            var userid = User.GetUserid();
            var result =await servicesCart.UpdateQuantityAsync(userid, dto);
            return result.ToActionResult(); ;
        }
    }
}

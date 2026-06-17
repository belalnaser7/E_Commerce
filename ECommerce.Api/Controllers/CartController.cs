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

            if (cart is null)
            {
                return NotFound();
            }

            var result = await authorizationService.AuthorizeAsync(
                User,
                cart,
                new CanViewCartRequirement());

            if (!result.Succeeded)
            {
                return Forbid();
            }

            var dto = servicesCart.GetCart(userId);

            return Ok(dto);
        }
        //for Customer 
        [Authorize(Roles = "Customer")]
        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userid = User.GetUserid();

            var cartitems = servicesCart.GetCart(userid);
            if (cartitems is null)
            {
                return NotFound();
            }

            return Ok(cartitems);
        }
        //For Admins 
        [HttpDelete("{cartitemid}/{userid}")]
        public async Task<IActionResult> DeleteCart(int cartitemid, string userid)
        {
            var cart = servicesCart.GetByUserId(userid);
            if (cart is null)
            {
                return NotFound();
            }
            var result = await authorizationService.AuthorizeAsync(User, cart, new CanViewCartRequirement());
            if (!result.Succeeded)
            {
                return Forbid();
            }
            var DeleteCart = servicesCart.RemoveItem(userid, cartitemid);
            if (!DeleteCart)
            {
                return NotFound();
            }
            return Ok();
        }
        //for Customer 
        [Authorize(Roles = "Customer")]
        [HttpDelete("{cartitemid}")]
        public async Task<IActionResult> DeleteCart(int cartitemid)
        {
            var userid = User.GetUserid();

            var DeleteCart = servicesCart.RemoveItem(userid, cartitemid);
            if (!DeleteCart)
            {
                return NotFound();
            }
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("clearCart/{userid}")]
        public async Task<IActionResult> ClearCart(string userid)
        {

            var result = servicesCart.ClearCart(userid);
            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> ClearCart()
        {
            var userid = User.GetUserid();
            var result = servicesCart.ClearCart(userid);
            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }

        [Authorize(Roles ="Customer")]
        [HttpPost]
        public async Task<IActionResult> AddToCart( AddToCartDto dto)
        {
            var userid = User.GetUserid();
            
            var addtocart = servicesCart.AddToCart(userid, dto);
            if (!addtocart)
            {
                return BadRequest();
            }
            return Ok();
        }
        [Authorize(Roles = "Customer")]
        [HttpPut]
        public async Task<IActionResult> Update(UpdateCartItemDto dto)
        {
            var userid = User.GetUserid();
            
            var addtocart = servicesCart.UpdateQuantity(userid, dto);
            if (!addtocart)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}

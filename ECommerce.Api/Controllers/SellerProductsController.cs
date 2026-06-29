using ECommerce.Api.Extentions;
using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Application.Police;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SellerProductsController : ControllerBase
    {
        private readonly IServicesProduct servicesProduct;
        private readonly IAuthorizationService authorizationService;

        public SellerProductsController(IServicesProduct servicesProduct, IAuthorizationService authorizationService)
        {
            this.servicesProduct = servicesProduct;
            this.authorizationService = authorizationService;
        }

        [Authorize(Roles = "Seller")]
        [HttpGet("products")]
        public async Task<IActionResult> GetSellerProducts()
        {
            string sellerId = User.GetUserid();

            var result = await servicesProduct.GetSellerProductsAsync(sellerId);

            return result.ToActionResult();
        }

        [HttpGet("product/{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var product = await servicesProduct.GetEntityByIdAsync(id);
            if (!product.IsSuccess)
            {
                return product.ToActionResult();
            }

            var result = await authorizationService.AuthorizeAsync(User, product.Data, new CanMangeSellerProductRequirement());
            if (!result.Succeeded)
            {
                return Forbid();
            }

            return product.ToActionResult();
        }

        [Authorize(Roles = "Seller")]
        [HttpPost("Product")]
        public async Task<IActionResult> Add(CreateProductDto dto)
        {
            string Sellerid = User.GetUserid();
            var result = await servicesProduct.AddAsync(dto, Sellerid);
            return result.ToActionResult();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateProductDto dto)
        {
            var product = await servicesProduct.GetEntityByIdAsync(id);
            if (!product.IsSuccess)
            {
                return product.ToActionResult();
            }

            var result = await authorizationService.AuthorizeAsync(User, product.Data, new CanMangeSellerProductRequirement());
            if (!result.Succeeded)
            {
                return Forbid();
            }
            var update = await servicesProduct.UpdateAsync(id, dto);
            return update.ToActionResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await servicesProduct.GetEntityByIdAsync(id);
            if (!product.IsSuccess)
            {
                return product.ToActionResult();
            }
            var result = await authorizationService.AuthorizeAsync(User, product.Data, new CanMangeSellerProductRequirement());
            if (!result.Succeeded)
            {
                return Forbid();
            }
            var delete = await servicesProduct.DelAsync(id);
            return delete.ToActionResult();
        }


    }
}

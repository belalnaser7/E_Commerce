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

    public class ProductsController : ControllerBase
    {
        private readonly IServicesProduct servicesProduct;
        private readonly IAuthorizationService authorizationService;

        public ProductsController(IServicesProduct servicesProduct,IAuthorizationService authorizationService)
        {
            this.servicesProduct = servicesProduct;
            this.authorizationService = authorizationService;
        }
        [Authorize("CanShow")]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await servicesProduct.GetAllAsync();
            return result.ToActionResult();
        }

        [Authorize("CanShow")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByidAsync(int id)
        {
            var result =await servicesProduct.GetByIdAsync(id);
            return result.ToActionResult();
        }

        [Authorize("CanManageProducts")]
        [HttpPost]
        public async Task<IActionResult> Add(CreateProductDto dto)
        {
            string Sellerid = User.GetUserid();
            var result =await servicesProduct.AddAsync(dto, Sellerid);
            return result.ToActionResult();
        }
      
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,UpdateProductDto dto)
        {
            var product =await servicesProduct.GetEntityByIdAsync(id);
            if (!product.IsSuccess)
            {
                return product.ToActionResult();
            }

            var result = await authorizationService.AuthorizeAsync(User, product.Data, new CanDeleteOrUpdateProductRequirement());
            if (!result.Succeeded)
            {
                return Forbid();
            }
            var update=await servicesProduct.UpdateAsync(id, dto);
            return update.ToActionResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product =await servicesProduct.GetEntityByIdAsync(id);
            if (!product.IsSuccess)
            {
                return product.ToActionResult();
            }
            var result = await authorizationService.AuthorizeAsync(User, product.Data, new CanDeleteOrUpdateProductRequirement());
            if (!result.Succeeded)
            {
                return Forbid();
            }
            var delete=await servicesProduct.DelAsync(id);
            return delete.ToActionResult();
        }
    }
}

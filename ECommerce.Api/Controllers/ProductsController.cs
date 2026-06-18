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
        public IActionResult GetAll()
        {
            var result = servicesProduct.GetAll();
            return result.ToActionResult();
        }

        [Authorize("CanShow")]
        [HttpGet("{id}")]
        public IActionResult GetByid(int id)
        {
            var result = servicesProduct.GetById(id);
            return result.ToActionResult();
        }

        [Authorize("CanManageProducts")]
        [HttpPost]
        public IActionResult Add(CreateProductDto dto)
        {
            string Sellerid = User.GetUserid();
            var result = servicesProduct.Add(dto, Sellerid);
            return result.ToActionResult();
        }
      
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,UpdateProductDto dto)
        {
            var product = servicesProduct.GetEntityById(id);
            if (!product.IsSuccess)
            {
                return product.ToActionResult();
            }

            var result = await authorizationService.AuthorizeAsync(User, product.Data, new CanDeleteOrUpdateProductRequirement());
            if (!result.Succeeded)
            {
                return Forbid();
            }
            var update=servicesProduct.Update(id, dto);
            return update.ToActionResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = servicesProduct.GetEntityById(id);
            if (!product.IsSuccess)
            {
                return product.ToActionResult();
            }
            var result = await authorizationService.AuthorizeAsync(User, product.Data, new CanDeleteOrUpdateProductRequirement());
            if (!result.Succeeded)
            {
                return Forbid();
            }
            var delete=servicesProduct.Del(id);
            return delete.ToActionResult();
        }
    }
}

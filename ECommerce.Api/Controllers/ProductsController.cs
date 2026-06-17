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
            var products = servicesProduct.GetAll();
            return Ok(products);
        }

        [Authorize("CanShow")]
        [HttpGet("{id}")]
        public IActionResult GetByid(int id)
        {
            var product = servicesProduct.GetById(id);
            if (product is null)
                return NotFound();
            return Ok(product);
        }

        [Authorize("CanManageProducts")]
        [HttpPost]
        public IActionResult Add(CreateProductDto dto)
        {
            string Sellerid = User.GetUserid();
            var result = servicesProduct.Add(dto, Sellerid);
            if (!result)
                return BadRequest();

            return Ok();

        }
      
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,UpdateProductDto dto)
        {
            var product = servicesProduct.GetEntityById(id);
            if (product is null)
            {
                return NotFound();
            }

            var result = await authorizationService.AuthorizeAsync(User, product, new CanDeleteOrUpdateProductRequirement());
            if (!result.Succeeded)
            {
                return Forbid();
            }
            var update=servicesProduct.Update(id, dto);
            if (!update)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = servicesProduct.GetEntityById(id);
            if (product is null)
            {
                return NotFound();
            }
            var result = await authorizationService.AuthorizeAsync(User, product, new CanDeleteOrUpdateProductRequirement());
            if (!result.Succeeded)
            {
                return Forbid();
            }
            servicesProduct.Del(product);
            return Ok();
        }
    }
}

using ECommerce.Api.Extentions;
using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CustomerProductsController : ControllerBase
    {
        private readonly IServicesProduct servicesProduct;
        public CustomerProductsController(IServicesProduct servicesProduct)
        {
            this.servicesProduct = servicesProduct;
        }
        [Authorize(Roles ="Customer")]
        [HttpGet("products")]
        public async Task<IActionResult> GetAllApprovedAsync()
        {
            var result = await servicesProduct.GetApprovedProductsAsync();
            return result.ToActionResult();
        }

        [Authorize(Roles = "Customer")]
        [HttpGet("product/{id}")]
        public async Task<IActionResult> GetApprovedByidAsync(int id)
        {
            var result =await servicesProduct.GetApprovedProductByIdAsync(id);
            return result.ToActionResult();
        }
        
    }
}

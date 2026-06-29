using ECommerce.Api.Extentions;
using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Domain_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminProductsController : ControllerBase
    {
        private readonly IServicesProduct servicesProduct;
       

        public AdminProductsController(IServicesProduct servicesProduct)
        {
            this.servicesProduct = servicesProduct;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("products")]
        public async Task<IActionResult> GetAllPendingAsync()
        {
            var result = await servicesProduct.GetPendingProductsAsync();
            return result.ToActionResult();
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("products/{id}")]
        public async Task<IActionResult> GetPendingProductById(int id)
        {
            var result = await servicesProduct.GetProductByStatusAsync(id, ProductStatus.Pending);
            return result.ToActionResult();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> ChangeStatus(int id, ChangeProductStatusDto status)
        {
            var result = await servicesProduct.ChangeProductStatusAsync(id, status);
            return result.ToActionResult();
        }
    }
}

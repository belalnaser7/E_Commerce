using ECommerce.Api.Extentions;
using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuzController : ControllerBase
    {
        private readonly IServicesAuz servicesRegister;

        public AuzController(IServicesAuz servicesRegister)
        {
            this.servicesRegister = servicesRegister;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> register(RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result=await servicesRegister.Register(dto);
            return result.ToActionResult();
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await servicesRegister.Login(login);

            return result.ToActionResult();
        }
    }
}

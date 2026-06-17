using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }
            return Ok();
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await servicesRegister.Login(login);
            if (result is null)
            {
                return BadRequest();
            }
            return Ok(result);

        }

        //[Authorize(Roles = "Customer")]
        //[HttpGet("admin")]
        //public IActionResult AdminOnly()
        //{
        //    return Ok("Welcome Admin");
        //}


    }
}

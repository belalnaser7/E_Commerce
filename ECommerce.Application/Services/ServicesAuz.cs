using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Domain_Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ECommerce.Application.Services
{
    public class ServicesAuz : IServicesAuz
    {
        private readonly UserManager<ApplicationUser> userManager;

        public IConfiguration Config { get; }

        public ServicesAuz(UserManager<ApplicationUser> userManager, IConfiguration config)
        {
            this.userManager = userManager;
            Config = config;
        }

        public async Task<RegisterResultDto> Register(RegisterDto register)
        {

            ApplicationUser user = new ApplicationUser()
            {
                UserName = register.UserName,
                Email = register.Email,
            };
            var create = await userManager.CreateAsync(user, register.PassWord);
            
            if (create.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Customer"); 
                return new RegisterResultDto
                {
                    Success = true
                };
            }
            return new RegisterResultDto
            {
                Success=false,
                Errors=create.Errors.Select(e=>e.Description).ToList()

            };
        }

        public async Task<LoginResponseDto?> Login(LoginDto login)
        {
            var user = await userManager.FindByNameAsync(login.UserName);
            if (user is null)
            {
                return null;
            }
            var checkpass = await userManager.CheckPasswordAsync(user, login.PassWord);
            if (!checkpass)
            {
                return null;
            }
            // هنعمل الكلايمز
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Email,user.Email??""),
                new Claim(ClaimTypes.Name,user.UserName??"")

            };
            //هنا هعمل id مميز لكن توكن واضيفه في ال claims عشان لو احتجته فيما بعد في اي حاجه زي اعمل بلوك للنوكن ده او اي حاجه 
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            // الجزء الخاص بال Roles

            var roles = await userManager.GetRolesAsync(user);
            foreach (var item in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, item));
            }

               var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config["Jwt:Key"]));
         // هروحج اعمل ال Key الي symmitricSecurtykey

            // هنعمل ال singingCredentials

            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // هروح احط ال jwt داخل ال Token

            var token = new JwtSecurityToken
                (
                issuer: Config["Jwt:Issuer"],
                audience: Config["Jwt:Audience"],
                expires: DateTime.UtcNow.AddHours(1),
                claims: claims,
                signingCredentials: credentials
                );
            //طيب عاوزين محول من التوكن من اوبجيكت الي سترينج 

            var TokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return new LoginResponseDto()
            {
                Token = TokenString,
                UserName =user.UserName

            };

        }
    }
}

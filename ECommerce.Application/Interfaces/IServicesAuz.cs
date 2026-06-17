using ECommerce.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces
{
    public interface IServicesAuz
    {
        Task<RegisterResultDto> Register(RegisterDto register);
        Task<LoginResponseDto?> Login(LoginDto login);
    }
}

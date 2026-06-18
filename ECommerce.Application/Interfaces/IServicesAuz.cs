using ECommerce.Application.DTOs;
using ECommerce.Application.Result_pattern;

namespace ECommerce.Application.Interfaces
{
    public interface IServicesAuz
    {
        Task<Result> Register(RegisterDto register);
        Task<Result<LoginResponseDto?>> Login(LoginDto login);
    }
}

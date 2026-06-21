using ECommerce.Application.DTOs;
using FluentValidation;

namespace ECommerce.Application.Validators
{
    public class LoginDtoValidation:AbstractValidator<LoginDto>
    {
        public LoginDtoValidation()
        {
            RuleFor(x => x.UserName)
               .NotEmpty();

            RuleFor(x => x.PassWord)
                .NotEmpty();
        }
    }
}

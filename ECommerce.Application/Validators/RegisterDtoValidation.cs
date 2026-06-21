using ECommerce.Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Validators
{
    public class RegisterDtoValidation:AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidation()
        {
            RuleFor(x => x.UserName)
               .NotEmpty()
               .MinimumLength(3)
               .MaximumLength(50);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.PassWord)
                .NotEmpty()
                .MinimumLength(6)
                .MaximumLength(100);

        }
    }
}

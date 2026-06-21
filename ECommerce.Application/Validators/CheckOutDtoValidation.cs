using ECommerce.Application.DTOs;
using FluentValidation;

namespace ECommerce.Application.Validators
{
    public class CheckOutDtoValidation:AbstractValidator<CheckOutDto>
    {
        public CheckOutDtoValidation()
        {
            RuleFor(s => s.ShippingAddress).NotEmpty().MaximumLength(50).MinimumLength(10);

        }
    }
}

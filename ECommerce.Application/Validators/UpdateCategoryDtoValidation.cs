using ECommerce.Application.DTOs;
using FluentValidation;

namespace ECommerce.Application.Validators
{
    public class UpdateCategoryDtoValidation:AbstractValidator<UpdateCategotyDto>
    {
        public UpdateCategoryDtoValidation()
        {
            RuleFor(o => o.Name).NotEmpty().MaximumLength(20);
            RuleFor(o => o.Description).MaximumLength(50);
        }
    }
}

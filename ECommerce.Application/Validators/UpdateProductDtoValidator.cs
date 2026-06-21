using ECommerce.Application.DTOs;
using FluentValidation;

namespace ECommerce.Application.Validators
{
    public class UpdateProductDtoValidator:AbstractValidator<UpdateProductDto>
    {
        public UpdateProductDtoValidator()
        {
            RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

            RuleFor(x => x.Price)
                .GreaterThan(0);

            RuleFor(x => x.StockQuantity)
               .GreaterThan(0);

            RuleFor(x => x.Description)
                .MaximumLength(500);
        }
    }
}

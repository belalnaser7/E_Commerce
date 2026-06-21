using ECommerce.Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Validators
{
    public class AddToCartDtoValidation:AbstractValidator<AddToCartDto>
    {
        public AddToCartDtoValidation()
        {
            RuleFor(o => o.Quantity).GreaterThan(0).LessThan(500);
        }
    }
}

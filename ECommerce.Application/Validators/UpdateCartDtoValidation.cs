using ECommerce.Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Validators
{
    public class UpdateCartDtoValidation:AbstractValidator<UpdateCartItemDto>
    {
        public UpdateCartDtoValidation()
        {
            RuleFor(o => o.Quantity).GreaterThan(0).LessThan(500);
        }
    }
}

using ECommerce.Application.DTOs;
using ECommerce.Application.Result_pattern;
using ECommerce.Domain.Domain_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces
{
    public interface IServicesCart
    {
        Result<Cart?> GetByUserId(string userId);
        Result<CartDto?> GetCart(string userId);

        Result AddToCart(string userId, AddToCartDto dto);

        Result RemoveItem(string userId, int cartitemid);

        Result UpdateQuantity(string userId, UpdateCartItemDto dto);

        Result ClearCart(string userId);




    }
}

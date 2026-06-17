using ECommerce.Application.DTOs;
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
        public Cart? GetByUserId(string userId);
        CartDto? GetCart(string userId);

        bool AddToCart(string userId, AddToCartDto dto);

        bool RemoveItem(string userId, int cartitemid);

        bool UpdateQuantity(string userId, UpdateCartItemDto dto);

        bool ClearCart(string userId);



        
    }
}

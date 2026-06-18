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
        Task<Result<Cart?>> GetByUserIdAsync(string userId);
        Task<Result> AddToCartAsync(string userId, AddToCartDto dto);

        Task<Result> ClearCartAsync(string userId);

        Task<Result<CartDto?>> GetCartAsync(string userId);

        Task<Result> RemoveItemAsync(string userId, int cartitemid);

        Task<Result> UpdateQuantityAsync(string userId, UpdateCartItemDto dto);




    }
}

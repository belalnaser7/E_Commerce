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
    public interface IServicesOrder
    {
         Result<Cart?> GetCartByUserId(string userId);
        Result<Order?> GetEntityById(int orderId);
         Result Checkout(string userId, CheckOutDto dto);

         Result<OrderDto?> GetOrderById(int orderId);

        Result<List<OrderDto>> GetOrders(string userId);
    }
}

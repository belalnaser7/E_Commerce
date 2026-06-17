using ECommerce.Application.DTOs;
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
        public Cart? GetCartByUserId(string userId);
        public Order? GetEntityById(int orderId);
        bool Checkout(string userId, CheckOutDto dto);

        List<OrderDto> GetOrders(string userId);

        OrderDto? GetOrderById(int orderId);
    }
}

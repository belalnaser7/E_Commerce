using ECommerce.Domain.Domain_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces
{
    public interface IRepositoryOrder
    {
        Task AddAsync(Order order);
        Task<Order?> GetByIdAsync(int orderid);
        Task<List<Order>?> GetByUserIdAsync(string userId);
       

    }
}

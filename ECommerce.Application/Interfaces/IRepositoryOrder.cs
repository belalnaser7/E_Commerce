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
        void Add(Order order);
        List<Order> GetByUserId(string userId);
        Order? GetById(int orderid);
        void Save();

    }
}

using ECommerce.Application.Interfaces;
using ECommerce.Domain.Domain_Models;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories
{
    public class RepositoryOrder:IRepositoryOrder
    {
        private readonly E_commerceDbcontext context;

        public RepositoryOrder(E_commerceDbcontext context)
        {
            this.context = context;
        }

        public void Add(Order order)
        {
            context.Add(order);
        }

        public Order? GetById(int orderid)
        {
            return context.Orders.Include(o => o.Items).ThenInclude(p=>p.Product).FirstOrDefault(i => i.Id == orderid);
        }

        public List<Order>? GetByUserId(string userId)
        {
            return context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .Where(o => o.UserId == userId)
                .ToList();
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}

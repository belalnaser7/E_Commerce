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

        public async Task AddAsync(Order order)
        {
           await context.AddAsync(order);
        }

        public async Task<Order?> GetByIdAsync(int orderid)
        {
            return await context.Orders.Include(o => o.Items).ThenInclude(p=>p.Product).FirstOrDefaultAsync(i => i.Id == orderid);
        }

        public async Task<List<Order>?> GetByUserIdAsync(string userId)
        {
            return await context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }

        public async Task SaveAsync()
        {
           await context.SaveChangesAsync();
        }
    }
}

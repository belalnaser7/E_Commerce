using ECommerce.Application.Interfaces;
using ECommerce.Domain.Domain_Models;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories
{
    public class RepositoryCart : IRepositoryCart
    {
        private readonly E_commerceDbcontext context;

        public RepositoryCart(E_commerceDbcontext context)
        {
            this.context = context;
        }

        public Task<Cart?> GetByUserIdAsync(string userId)
        {
            return context.Carts
                .Include(c => c.Items)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public Task<Cart?> GetByIdAsync(int cartId)
        {
            return context.Carts
                .Include(c => c.Items)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.Id == cartId);
        }

        public async Task AddAsync(Cart cart)
        {
            await context.Carts.AddAsync(cart);
        }

    
        public void Remove(CartItem cart)
        {
            context.Remove(cart);
        }
    }
}

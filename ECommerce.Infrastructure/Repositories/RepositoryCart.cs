using ECommerce.Application.DTOs;
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

        public Cart? GetByUserId(string userId)
        {
            return context.Carts
                .Include(c => c.Items)
                    .ThenInclude(i => i.Product)
                .FirstOrDefault(c => c.UserId == userId);
        }

        public Cart? GetById(int cartId)
        {
            return context.Carts
                .Include(c => c.Items)
                    .ThenInclude(i => i.Product)
                .FirstOrDefault(c => c.Id == cartId);
        }

        public void Add(Cart cart)
        {
            context.Carts.Add(cart);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Remove(CartItem cart)
        {
            context.Remove(cart);
        }
    }
}

using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Domain_Models;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories
{
    public class RepositoryProduct:IRepositoryProduct
    {
        private readonly E_commerceDbcontext context;

        public RepositoryProduct(E_commerceDbcontext context)
        {
            this.context = context;
        }

        public async Task AddAsync(Product dto)
        {
             await context.AddAsync(dto);
        }

        public void Del(Product product)
        {
            context.Remove(product);
        }

        public  async Task<IEnumerable<Product>> GetAllAsync()=>
        await context.Products.ToListAsync();
        

        public Task<Product?> GetByIdAsync(int id)
        {
            return context.Products.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task SaveAsync()
        {
           await context.SaveChangesAsync();
        }

    
    }
}

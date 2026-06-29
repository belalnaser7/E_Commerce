
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

        public async Task<IEnumerable<Product>> GetByStatusAsync(ProductStatus productStatus) 
        {
            return await context.Products.Where(status=>status.Status==productStatus).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetBySellerIdAsync(string sellerId)
        {
            return await context.Products.Where(i=>i.SellerId==sellerId).ToListAsync();
        }

        public Task<Product?> GetByIdAsync(int id)
        {
            return context.Products.FirstOrDefaultAsync(i => i.Id == id);
        }
        public Task<Product?> GetByStatusIdAsync(int id, ProductStatus productStatus)
        {
            return context.Products.Where(status => status.Status == productStatus).FirstOrDefaultAsync(i => i.Id == id);
        }

        //public Task<Product?> UpdateStatusOnlyAdmins(int id)
        //{
        //    return context.Products.FirstOrDefaultAsync(i => i.Id == id);
        //}



    }
}

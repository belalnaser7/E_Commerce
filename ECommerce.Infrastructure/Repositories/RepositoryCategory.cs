using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Domain_Models;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories
{
    public class RepositoryCategory : IRepositoryCategory
    {
        private readonly E_commerceDbcontext context;

        public RepositoryCategory(E_commerceDbcontext Context)
        {
            context = Context;
        }

        public async Task<IEnumerable<Category>> GetAllAsync() =>
                await context.Categories.ToListAsync();
        public async Task<Category?> GetByIdAsync(int id) =>
          await context.Categories.FirstOrDefaultAsync(d => d.Id == id);
        public async Task<bool> IsExistAsync(string name) =>

            await context.Categories.AnyAsync(k => k.Name == name);

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }

        public async Task AddAsync(Category dto) =>
           await context.AddAsync(dto);


        public void Del(Category category)
        {

            context.Remove(category);

        }

    }
}

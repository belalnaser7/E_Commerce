using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Domain_Models;
using ECommerce.Infrastructure.Data;

namespace ECommerce.Infrastructure.Repositories
{
    public class RepositoryCategory : IRepositoryCategory
    {
        private readonly E_commerceDbcontext context;

        public RepositoryCategory(E_commerceDbcontext Context)
        {
            context = Context;
        }

        public IEnumerable<Category> GetAll() =>
                  context.Categories.ToList();

        public Category? GetById(int id) =>
           context.Categories.FirstOrDefault(d => d.Id == id);


        public void Save() =>
            context.SaveChanges();

        public void Add(Category dto)=>
            context.Add(dto);
        

        public void Del(Category category)
        {
            
                context.Remove(category);
          
        }

       
        //public bool Update(int id ,Category category)
        //{
        //    var Found = GetById(id);
        //    if (Found is null)
        //        return false;
        //    Found.Name = category.Name;
        //    Found.Description = category.Description;
        //    return true;
        //}
    }
}

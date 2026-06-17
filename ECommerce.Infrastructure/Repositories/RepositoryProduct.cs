using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Domain_Models;
using ECommerce.Infrastructure.Data;

namespace ECommerce.Infrastructure.Repositories
{
    public class RepositoryProduct:IRepositoryProduct
    {
        private readonly E_commerceDbcontext context;

        public RepositoryProduct(E_commerceDbcontext context)
        {
            this.context = context;
        }

        public void Add(Product dto)
        {
            context.Add(dto);
        }

        public void Del(Product product)
        {

            context.Remove(product);

        }

        public IEnumerable<Product> GetAll()
        {
            return context.Products.ToList();
        }

        public Product GetById(int id)
        {
            return context.Products.FirstOrDefault(i => i.Id == id);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        //public bool Update(int id , Product product)
        //{
        //    var Found = GetById(id);
        //    if (Found is null)
        //    {
        //        return false;
        //    }
        //    Found.Name = product.Name;
        //    Found.Price = product.Price;
        //    Found.Description = product.Description;
        //    Found.StockQuantity = product.StockQuantity;

        //    return true;
        //}
    }
}

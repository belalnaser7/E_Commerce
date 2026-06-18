using ECommerce.Application.DTOs;
using ECommerce.Domain.Domain_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces
{
    public interface IRepositoryProduct
    {
        Task AddAsync(Product dto);
        void Del(Product product);

        Task<IEnumerable<Product>> GetAllAsync();
        //public bool Update(int id, Product product);
        Task<Product?> GetByIdAsync(int id);
        Task SaveAsync();
    }
}

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

        Task<IEnumerable<Product>> GetByStatusAsync(ProductStatus productStatus);
        Task<IEnumerable<Product>> GetBySellerIdAsync(string sellerId);
        Task<Product?> GetByStatusIdAsync(int id, ProductStatus productStatus);
        Task<Product?> GetByIdAsync(int id);
       
    }
}

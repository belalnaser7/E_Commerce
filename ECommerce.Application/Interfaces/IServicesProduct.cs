using ECommerce.Application.DTOs;
using ECommerce.Domain.Domain_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces
{
    public interface IServicesProduct
    {
        IEnumerable<ProductDto> GetAll();
        ProductDto? GetById(int id);
        Product? GetEntityById(int id);
        void Del(Product product1);
        bool Update(int id, UpdateProductDto dto);
        bool Add(CreateProductDto dto, string Sellerid);

    }
}

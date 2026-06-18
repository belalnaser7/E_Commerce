using ECommerce.Application.DTOs;
using ECommerce.Application.Result_pattern;
using ECommerce.Domain.Domain_Models;

namespace ECommerce.Application.Interfaces
{
    public interface IServicesProduct
    {
        Result<IEnumerable<ProductDto>> GetAll();
        Result<ProductDto?> GetById(int id);
        Result<Product?> GetEntityById(int id);
        Result Del(int id);
        Result Update(int id, UpdateProductDto dto);
        Result Add(CreateProductDto dto, string Sellerid);
    }
}

using ECommerce.Application.DTOs;
using ECommerce.Application.Result_pattern;
using ECommerce.Domain.Domain_Models;

namespace ECommerce.Application.Interfaces
{
    public interface IServicesProduct
    {
        Task<Result> AddAsync(CreateProductDto dto, string Sellerid);
        Task<Result> DelAsync(int id);
        Task<Result<ProductDto?>> GetByIdAsync(int id);
        Task<Result<Product?>> GetEntityByIdAsync(int id);
        Task<Result> UpdateAsync(int id, UpdateProductDto dto);
        Task<Result<Product?>> GetProductByStatusAsync(int id, ProductStatus status);
        Task<Result<IEnumerable<ProductDto>>> GetApprovedProductsAsync();
        Task<Result<IEnumerable<ProductDto>>> GetPendingProductsAsync();
        Task<Result> ChangeProductStatusAsync(int id, ChangeProductStatusDto status);
        Task<Result<ProductDto?>> GetApprovedProductByIdAsync(int id);
        Task<Result<IEnumerable<ProductDto>>> GetSellerProductsAsync(string sellerId);
    }
}

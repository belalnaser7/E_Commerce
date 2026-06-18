using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Application.Result_pattern;
using ECommerce.Domain.Domain_Models;
using Mapster;

namespace ECommerce.Application.Services
{
    public class ServicesProduct : IServicesProduct
    {
        private readonly IRepositoryProduct product;
        public ServicesProduct(IRepositoryProduct product)
        {
            this.product = product;
        }

        public async Task<Result> AddAsync(CreateProductDto dto, string Sellerid)
        {
            if (dto is null)
                return Result.Fail("Invalid request");
            if (string.IsNullOrWhiteSpace(dto.Name))
                return Result.Fail("The Product Name isn't Valid");
            if (dto.Price <= 0)
                return Result.Fail("The Product Price isn't Valid");
            if (dto.StockQuantity <= 0)
                return Result.Fail("The Product Quantity isn't Valid");
            var pro = dto.Adapt<Product>();
            pro.SellerId = Sellerid;
           await product.AddAsync(pro);
           await product.SaveAsync();
            return Result.Success();
        }

        public async Task<Result> DelAsync(int id)
        {
            var product1 =await product.GetByIdAsync(id);
            if (product1 is null)
                return Result.Fail("The Product isn't Exsit");
            product.Del(product1);
           await product.SaveAsync();
            return Result.Success();
        }

        public async Task<Result<IEnumerable<ProductDto>>> GetAllAsync()
        {
            var products =await product.GetAllAsync();
            var dto = products.Adapt<List<ProductDto>>();

            return Result<IEnumerable<ProductDto>>.Success(dto);
        }

        public async Task<Result<ProductDto?>> GetByIdAsync(int id)
        {
            var product1 =await product.GetByIdAsync(id);
            if (product1 is null)
                return Result<ProductDto?>.Fail("The Product isn't Exsit");
            var dto = product1.Adapt<ProductDto>();
            return Result<ProductDto?>.Success(dto);
        }

        public async Task<Result<Product?>> GetEntityByIdAsync(int id) // helper
        {
            var product1 =await product.GetByIdAsync(id);
            if (product1 is null)
                return Result<Product?>.Fail("The Product isn't Exsit",ErrorType.NotFound);
            return Result<Product?>.Success(product1);
        }

        public async Task<Result> UpdateAsync(int id, UpdateProductDto dto)
        {
            var Found =await GetEntityByIdAsync(id);
            if (!Found.IsSuccess)
                return Found;
            if (string.IsNullOrWhiteSpace(dto.Name))
                return Result.Fail("The Product Name isn't Valid");
            if (dto.Price <= 0)
                return Result.Fail("The Product Price isn't Valid");
            if (dto.StockQuantity < 0)
                return Result.Fail("The Product Quantity isn't Valid");
            dto.Adapt(Found.Data);
            await product.SaveAsync();
            return Result.Success();
        }
    }
}

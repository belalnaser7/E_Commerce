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

        public Result Add(CreateProductDto dto, string Sellerid)
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
            product.Add(pro);
            product.Save();
            return Result.Success();
        }

        public Result Del(int id)
        {
            var product1 = product.GetById(id);
            if (product1 is null)
                return Result.Fail("The Product isn't Exsit");
            product.Del(product1);
            product.Save();
            return Result.Success();
        }

        public Result<IEnumerable<ProductDto>> GetAll()
        {
            var products = product.GetAll();
            var dto = products.Adapt<List<ProductDto>>();

            return Result<IEnumerable<ProductDto>>.Success(dto);
        }

        public Result<ProductDto?> GetById(int id)
        {
            var product1 = product.GetById(id);
            if (product1 is null)
                return Result<ProductDto?>.Fail("The Product isn't Exsit");
            var dto = product1.Adapt<ProductDto>();
            return Result<ProductDto?>.Success(dto);
        }

        public Result<Product?> GetEntityById(int id) // helper
        {
            var product1 = product.GetById(id);
            if (product1 is null)
                return Result<Product?>.Fail("The Product isn't Exsit",ErrorType.NotFound);
            return Result<Product?>.Success(product1);
        }

        public Result Update(int id, UpdateProductDto dto)
        {
            var Found = GetEntityById(id);
            if (!Found.IsSuccess)
                return Found;
            if (string.IsNullOrWhiteSpace(dto.Name))
                return Result.Fail("The Product Name isn't Valid");
            if (dto.Price <= 0)
                return Result.Fail("The Product Price isn't Valid");
            if (dto.StockQuantity < 0)
                return Result.Fail("The Product Quantity isn't Valid");
            dto.Adapt(Found.Data);
            product.Save();
            return Result.Success();
        }
    }
}

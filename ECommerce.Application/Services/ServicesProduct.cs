using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Application.Result_pattern;
using ECommerce.Domain.Domain_Models;
using Mapster;

namespace ECommerce.Application.Services
{
    public class ServicesProduct : IServicesProduct
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly ICacheService cacheService;

        public ServicesProduct(IUnitOfWork unitOfWork, ICacheService cacheService)
        {

            this.unitOfWork = unitOfWork;
            this.cacheService = cacheService;
        }

        public async Task<Result> AddAsync(CreateProductDto dto, string Sellerid)
        {
            //if (dto is null)
            //    return Result.Fail("Invalid request");
            //if (string.IsNullOrWhiteSpace(dto.Name))
            //    return Result.Fail("The Product Name isn't Valid");
            //if (dto.Price <= 0)
            //    return Result.Fail("The Product Price isn't Valid");
            //if (dto.StockQuantity <= 0)
            //    return Result.Fail("The Product Quantity isn't Valid");
            var pro = dto.Adapt<Product>();
            pro.SellerId = Sellerid;
            await unitOfWork.Products.AddAsync(pro);
            await unitOfWork.SaveAsync();
            cacheService.Remove("Products");

            return Result.Success();
        }

        public async Task<Result> DelAsync(int id)
        {
            var product1 = await unitOfWork.Products.GetByIdAsync(id);
            if (product1 is null)
                return Result.Fail("The Product isn't Exsit");
            unitOfWork.Products.Del(product1);
            await unitOfWork.SaveAsync();
            cacheService.Remove("Products");
            cacheService.Remove($"Product:{id}");
            cacheService.Remove($"productEntity:{id}");
            return Result.Success();
        }

        public async Task<Result<IEnumerable<ProductDto>>> GetAllAsync()
        {
            string key = "Products";
            var productsDto = cacheService.Get<IEnumerable<ProductDto>>(key);
            if (productsDto is null)
            {
                var products = await unitOfWork.Products.GetAllAsync();
                productsDto = products.Adapt<List<ProductDto>>();
                cacheService.Set(key, TimeSpan.FromMinutes(30), productsDto);
            }
            return Result<IEnumerable<ProductDto>>.Success(productsDto);
        }

        public async Task<Result<ProductDto?>> GetByIdAsync(int id)
        {
            string key = $"Product:{id}";
            var productDto = cacheService.Get<ProductDto?>(key);
            if (productDto is null)
            {
                var product = await unitOfWork.Products.GetByIdAsync(id);
                if (product is null)
                    return Result<ProductDto?>.Fail("The Product isn't Exsit");
                productDto = product.Adapt<ProductDto>();
                cacheService.Set(key, TimeSpan.FromMinutes(30), productDto);
            }
            return Result<ProductDto?>.Success(productDto);
        }

        public async Task<Result<Product?>> GetEntityByIdAsync(int id) // helper
        {
            string key = $"productEntity:{id}";

            var product = cacheService.Get<Product?>(key);
            if (product is null)
            {
                var product1 = await unitOfWork.Products.GetByIdAsync(id);
                if (product1 is null)
                    return Result<Product?>.Fail("The Product isn't Exsit", ErrorType.NotFound);

                cacheService.Set(key, TimeSpan.FromMinutes(30), product1);
                product = product1;
            }

            return Result<Product?>.Success(product);
        }

        public async Task<Result> UpdateAsync(int id, UpdateProductDto dto)
        {
            var Found = await GetEntityByIdAsync(id);
            if (!Found.IsSuccess)
                return Found;
            //if (string.IsNullOrWhiteSpace(dto.Name))
            //    return Result.Fail("The Product Name isn't Valid");
            //if (dto.Price <= 0)
            //    return Result.Fail("The Product Price isn't Valid");
            //if (dto.StockQuantity < 0)
            //    return Result.Fail("The Product Quantity isn't Valid");
            dto.Adapt(Found.Data);
            await unitOfWork.SaveAsync();
            cacheService.Remove("Products");
            cacheService.Remove($"Product:{id}");
            cacheService.Remove($"productEntity:{id}");

            return Result.Success();
        }
    }
}

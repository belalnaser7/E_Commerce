using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Application.Result_pattern;
using ECommerce.Domain.Domain_Models;
using Mapster;

namespace ECommerce.Application.Services
{
    public class ServicesCategory : IServicesCategory
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly ICacheService cacheService;

        public ServicesCategory(IUnitOfWork unitOfWork, ICacheService cacheService)
        {
            this.unitOfWork = unitOfWork;
            this.cacheService = cacheService;
        }
        public async Task<Result> AddAsync(CreateCategoryDto dto)
        {

            if (await unitOfWork.Categorys.IsExistAsync(dto.Name))
            {
                return Result.Fail("The Name is Already Exsit");
            }

            var categoryEntity = dto.Adapt<Category>();
            await unitOfWork.Categorys.AddAsync(categoryEntity);
            await unitOfWork.SaveAsync();
            cacheService.Remove("Categories");
            return Result.Success();
        }
        public async Task<Result> DelAsync(int id)
        {
            var category1 = await unitOfWork.Categorys.GetByIdAsync(id);
            if (category1 is null)
                return Result.Fail("This Category isn't Exsit");
            unitOfWork.Categorys.Del(category1);
            await unitOfWork.SaveAsync();
            cacheService.Remove($"Category:{id}");
            cacheService.Remove("Categories");
            return Result.Success();
        }


        public async Task<Result<IEnumerable<CategoryDto>>> GetAllAsync()
        {
            string key = "Categories";
            var CategoriesDto = cacheService.Get<IEnumerable<CategoryDto>>(key);
            if (CategoriesDto is null)
            {
                Console.WriteLine("CACHE MISS");
                var CategoriesFromDatebase = await unitOfWork.Categorys.GetAllAsync();
                CategoriesDto = CategoriesFromDatebase.Adapt<List<CategoryDto>>();
                cacheService.Set(key, TimeSpan.FromHours(1), CategoriesDto);
            }
            Console.WriteLine("CACHE HIT");
            return Result<IEnumerable<CategoryDto>>.Success(CategoriesDto);
        }

        public async Task<Result<CategoryDto?>> GetByIdAsync(int id)
        {
            string key = $"Category:{id}";
            var categoryDto = cacheService.Get<CategoryDto>(key);
            if (categoryDto is null)
            {
                var categoryEntity = await unitOfWork.Categorys.GetByIdAsync(id);
                if (categoryEntity is null)
                {
                    return Result<CategoryDto?>.Fail("This Category isn't Exsit");
                }
                categoryDto = categoryEntity.Adapt<CategoryDto>();
                cacheService.Set(key, TimeSpan.FromHours(1), categoryDto);
            }

            return Result<CategoryDto?>.Success(categoryDto);
        }

        public async Task<Result> UpdateAsync(int id, UpdateCategotyDto dto)
        {
            var category2 = await unitOfWork.Categorys.GetByIdAsync(id);
            if (category2 is null)
                return Result.Fail("This Category isn't Exsit");
            dto.Adapt(category2);
            await unitOfWork.SaveAsync();
            cacheService.Remove($"Category:{id}");
            cacheService.Remove("Categories");
            return Result.Success();

        }
    }
}

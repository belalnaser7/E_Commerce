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

        public ServicesCategory(IUnitOfWork unitOfWork)
        { 
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result> AddAsync(CreateCategoryDto dto)
        {
            //if (string.IsNullOrWhiteSpace(dto.Name))
            //{
            //    return Result.Fail("The Name is Empty");
            //}

            if (await unitOfWork.Categorys.IsExistAsync(dto.Name))
            {
                return Result.Fail("The Name is Already Exsit");
            }

            var categoryEntity = dto.Adapt<Category>();
            await unitOfWork.Categorys.AddAsync(categoryEntity);
            await unitOfWork.SaveAsync();
            return Result.Success();
        }
        public async Task<Result> DelAsync(int id)
        {
            var category1 = await unitOfWork.Categorys.GetByIdAsync(id);
            if (category1 is null)
                return Result.Fail("This Category isn't Exsit");
            unitOfWork.Categorys.Del(category1);
            await unitOfWork.SaveAsync();
            return Result.Success();
        }

        public async Task<Result<IEnumerable<CategoryDto>>> GetAllAsync()
        {
            var Categories = await unitOfWork.Categorys.GetAllAsync();

            var dto = Categories.Adapt<List<CategoryDto>>();

            return Result<IEnumerable<CategoryDto>>.Success(dto);
        }

        public async Task<Result<CategoryDto?>> GetByIdAsync(int id)
        {
            var categoryEntity = await unitOfWork.Categorys.GetByIdAsync(id);
            if (categoryEntity is null)
            {
                return Result<CategoryDto?>.Fail("This Category isn't Exsit");
            }

            var dto = categoryEntity.Adapt<CategoryDto>();

            return Result<CategoryDto?>.Success(dto);
        }

        public async Task<Result> UpdateAsync(int id, UpdateCategotyDto dto)
        {
            var category2 = await unitOfWork.Categorys.GetByIdAsync(id);
            if (category2 is null)
                return Result.Fail("This Category isn't Exsit");
            dto.Adapt(category2);
            await unitOfWork.SaveAsync();
            return Result.Success();

        }
    }
}

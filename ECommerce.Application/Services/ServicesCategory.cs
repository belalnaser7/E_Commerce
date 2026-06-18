using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Application.Result_pattern;
using ECommerce.Domain.Domain_Models;
using Mapster;

namespace ECommerce.Application.Services
{
    public class ServicesCategory:IServicesCategory
    {
        private readonly IRepositoryCategory category;

        public ServicesCategory(IRepositoryCategory category)
        {
            this.category = category;
        }


        public Result Add(CreateCategoryDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return Result.Fail("The Name is Empty");
            }
           
            if (category.IsExist(dto.Name))
            {
                return Result.Fail("The Name is Already Exsit");
            }

            var categoryEntity = dto.Adapt<Category>();
            category.Add(categoryEntity);
            category.Save();
            return Result.Success();
        }
        public Result Del(int id)
        {
            var category1 = category.GetById(id);
            if (category1 is null)
                return Result.Fail("This Category isn't Exsit");
            category.Del(category1);
            category.Save();
            return Result.Success();
        }

        public Result<IEnumerable<CategoryDto>> GetAll()
        {
            var Categories = category.GetAll();
            
           var dto= Categories.Adapt<List<CategoryDto>>();

            return Result<IEnumerable<CategoryDto>?>.Success(dto);
        }

        public Result<CategoryDto?> GetById(int id)
        {
            var categoryEntity = category.GetById(id);
            if (categoryEntity is null)
            {
                return Result<CategoryDto?>.Fail("This Category isn't Exsit");
            }

            var dto = categoryEntity.Adapt<CategoryDto>();

            return Result<CategoryDto?>.Success(dto);
        }

        public Result Update(int id, UpdateCategotyDto dto)
        {
            var category2 = category.GetById(id);
            if (category2 is null)
                return Result.Fail("This Category isn't Exsit");
            dto.Adapt(category2);
            category.Save();
            return Result.Success();

        }
    }
}

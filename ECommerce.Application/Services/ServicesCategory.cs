using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
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

        public bool Add(CreateCategoryDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return false;

            var categoryEntity = dto.Adapt<Category>();
            category.Add(categoryEntity);
            category.Save();
            return true;
        }

        public bool Del(int id)
        {
            var category1 = category.GetById(id);
            if (category1 is null)
                return false;
            category.Del(category1);
            category.Save();
            return true;
        }

        public IEnumerable<CategoryDto> GetAll()
        {
            var Categories = category.GetAll();

            return Categories.Adapt<List<CategoryDto>>();
        }

        public CategoryDto? GetById(int id)
        {
            var categoryEntity = category.GetById(id);

            if (categoryEntity is null)
                return null;

            return categoryEntity.Adapt<CategoryDto>();
        }

        public bool Update(int id, UpdateCategotyDto dto)
        {
            var category2 = category.GetById(id);
            if (category2 is null)
                return false;
            dto.Adapt(category2);
            category.Save();
            return true;

        }
    }
}

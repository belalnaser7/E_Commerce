using ECommerce.Application.DTOs;
using ECommerce.Domain.Domain_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces
{
    public interface IServicesCategory
    {
        IEnumerable<CategoryDto> GetAll();
        CategoryDto? GetById(int id);
        bool Del(int id);
        bool Update(int id, UpdateCategotyDto dto);
        bool Add(CreateCategoryDto dto);

    }
}

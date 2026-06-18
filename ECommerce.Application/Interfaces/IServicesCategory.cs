using ECommerce.Application.DTOs;
using ECommerce.Application.Result_pattern;
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
        public Result<IEnumerable<CategoryDto>> GetAll();
        public Result<CategoryDto?> GetById(int id);
        Result Del(int id);
        Result Update(int id, UpdateCategotyDto dto);
        Result Add(CreateCategoryDto dto);

    }
}

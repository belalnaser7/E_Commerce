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
        Task<Result> AddAsync(CreateCategoryDto dto);
        Task<Result> DelAsync(int id);
        Task<Result<IEnumerable<CategoryDto>>> GetAllAsync();
        Task<Result<CategoryDto?>> GetByIdAsync(int id);
        Task<Result> UpdateAsync(int id, UpdateCategotyDto dto);

    }
}

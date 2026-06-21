using ECommerce.Application.DTOs;
using ECommerce.Domain.Domain_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces
{
    public interface IRepositoryCategory
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task<bool> IsExistAsync(string name);
     
        Task AddAsync(Category dto);
        void Del(Category category);

    }
}

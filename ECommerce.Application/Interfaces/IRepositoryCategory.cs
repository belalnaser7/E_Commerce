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
        public IEnumerable<Category> GetAll();
        public Category? GetById(int id);
        public void Del(Category category);
       // public bool Update(int id,Category category);
        public void Add(Category dto);
        public void Save();

    }
}

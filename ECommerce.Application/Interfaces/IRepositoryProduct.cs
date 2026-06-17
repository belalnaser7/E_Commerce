using ECommerce.Application.DTOs;
using ECommerce.Domain.Domain_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces
{
    public interface IRepositoryProduct
    {
        public IEnumerable<Product> GetAll();
        public Product GetById(int id);
       
        public void Del(Product product);
        //public bool Update(int id, Product product);
        public void Add(Product dto);
        public void Save();
    }
}

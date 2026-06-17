using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;

using ECommerce.Domain.Domain_Models;
using Mapster;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Application.Services
{
    public class ServicesProduct:IServicesProduct
    {
        private readonly IRepositoryProduct product;
        

        public ServicesProduct(IRepositoryProduct product)
        {
            this.product = product;
           
        }

        public bool Add(CreateProductDto dto,string Sellerid)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return false;
            if(dto.Price<=0)
                return false;
            if (dto.StockQuantity <= 0)
                return false;
            var pro = dto.Adapt<Product>();
            pro.SellerId = Sellerid;
            //pro.SellerId = "fadd5e60-e669-4013-821f-fdd5fed55245";
            product.Add(pro);
            product.Save();
            return true;
        }

        public void Del(Product product1)
        {
            product.Del(product1);
            product.Save(); 
        }

        public IEnumerable<ProductDto> GetAll()
        {
            var products = product.GetAll();

            return products.Adapt<List<ProductDto>>();
        }

        public ProductDto? GetById(int id)
        {
            var product1 = product.GetById(id);

            if (product1 is null)
                return null;
           

            return product1.Adapt<ProductDto>();
        }

        public Product? GetEntityById(int id) // helper
        {
            var product1 = product.GetById(id);

            if (product1 is null)
                return null;
            return product1;
        }

        public bool Update(int id, UpdateProductDto dto)
        {
            var Found = GetEntityById(id);
            if (Found is null)
                return false;
            if (string.IsNullOrWhiteSpace(dto.Name))
                return false;
            if (dto.Price <= 0)
                return false;
            if (dto.StockQuantity < 0)
                return false;
            dto.Adapt(Found);
            product.Save();

            return true;

        }
    }
}

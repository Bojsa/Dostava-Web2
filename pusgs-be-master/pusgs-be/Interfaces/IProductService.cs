using pusgs_be.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pusgs_be.Interfaces
{
    public interface IProductService
    {
        void AddProduct(ProductAddDto product);
        List<ProductDto> GetProducts();
        ProductDto GetProduct(int id);
        void DeleteProduct(int id);
    }
}

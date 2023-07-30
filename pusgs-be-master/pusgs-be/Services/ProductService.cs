using AutoMapper;
using pusgs_be.Dto;
using pusgs_be.Infrastructure;
using pusgs_be.Interfaces;
using pusgs_be.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pusgs_be.Services
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;

        public ProductService(IMapper mapper, DataContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public void AddProduct(ProductAddDto product)
        {
            if (_dbContext.Products.Any(x => x.Name == product.Name))
                throw new Exception("Product already exists!");

            Product p = _mapper.Map<Product>(product);
            _dbContext.Products.Add(p);
            _dbContext.SaveChanges();
        }

        public List<ProductDto> GetProducts()
        {
            return _mapper.Map<List<ProductDto>>(_dbContext.Products.ToList());
        }

        public ProductDto GetProduct(int id)
        {
            return _mapper.Map<ProductDto>(_dbContext.Products.Find(id));
        }

        public void DeleteProduct(int id)
        {
            Product p = _dbContext.Products.Find(id);
            if (p == null)
                throw new Exception("Product not found!");

            _dbContext.Products.Remove(p);
            _dbContext.SaveChanges();
        }
    }
}

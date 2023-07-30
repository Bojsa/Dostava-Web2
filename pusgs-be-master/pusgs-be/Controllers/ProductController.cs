using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pusgs_be.Dto;
using pusgs_be.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pusgs_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            try
            {
                return Ok(_productService.GetProducts());
            }
            catch
            {
                return StatusCode(500, "Internal Server Error!");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult AddProduct([FromBody] ProductAddDto product)
        {
            try
            {
                _productService.AddProduct(product);

                return StatusCode(201);
            }
            catch (Exception e)
            {
                if (e.Message == "Restaurant not found!")
                {
                    return StatusCode(404, "Restaurant not found!");
                }
                else if (e.Message == "Product already exists!")
                {
                    return StatusCode(409, "Product already exists!");
                }
                else
                {
                    return StatusCode(500, "Internal Server Error!");
                }
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            try
            {
                ProductDto product = _productService.GetProduct(id);
                if (product != null)
                {
                    return Ok(product);
                }
                else
                {
                    return StatusCode(404, "Product not found!");
                }
            }
            catch
            {
                return StatusCode(500, "Internal Server Error!");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public IActionResult DeleteProduct(int id)
        {
            try
            {
                _productService.DeleteProduct(id);

                return StatusCode(200);
            }
            catch (Exception e)
            {
                if (e.Message == "Product not found!")
                {
                    return StatusCode(404, e.Message);
                }
                else
                {
                    return StatusCode(500, "Internal Server Error!");
                }
            }
        }
    }
}

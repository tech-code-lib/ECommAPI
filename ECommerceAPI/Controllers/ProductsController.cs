using AutoMapper;
using ECommerceAPI.DTOs;
using ECommerceAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly ECommDBContext _dbContext;
        private readonly IMapper _mapper;
        public ProductsController(ECommDBContext context, IMapper mapper)
        {
            _dbContext = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAllProducts()
        {
            var products = _dbContext.Products;
            var productsDtos = products.Select(x => _mapper.Map<Product, ProductDTO>(x));
            return Ok(productsDtos);
        }

        [HttpGet("{id}")]
        public IActionResult GetProductDetailById(int id)
        {
            var product = _dbContext.Products.FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            var productDto = _mapper.Map<Product, ProductDTO>(product);
            return Ok(product);
        }

        [HttpGet]
        [Route("ProductsByCategory")]
        public IActionResult GetProductByCategoryId([FromQuery]int catId)
        {
            if (catId == 0)
            {
                return BadRequest();
            }
            var products = _dbContext.Products.Where(x => x.CategoryId == catId);
            if (products == null)
            {
                return NotFound();
            }

            var productsDto = products.Select(x => _mapper.Map<Product, ProductDTO>(x));
            return Ok(productsDto);
        }

        [HttpPost]        
        public IActionResult AddAProduct([FromBody] ProductDTO product)
        {
            if (product == null || string.IsNullOrEmpty(product.Name) || product.Price <= 0)
            {
                return BadRequest();
            }

            var productToAdd = _mapper.Map<ProductDTO, Product>(product);
            _dbContext.Products.Add(productToAdd);
            _dbContext.SaveChanges();

            return CreatedAtAction(nameof(GetProductDetailById), new { id = productToAdd.Id }, productToAdd);
        }
    }
}

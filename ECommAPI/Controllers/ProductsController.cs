using ECommAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        //Hardcoded values
        IEnumerable<Product> products = new List<Product>() { 
            new Product{ Id = 1, Name = "Lipton Tea", Description = "English Black Tea", Price = 1.5m },
            new Product{ Id = 2, Name = "Nescafe", Description = "Instant Coffee", Price = 4.5m },
            new Product{ Id = 3, Name = "Kinley Water", Description = "Mineral Water", Price = 1.0m },
            new Product{ Id = 4, Name = "Hershey's", Description = "Chocolate", Price = 1.5m },
        };

        public ProductsController()
        {

        }
        
        [HttpGet]
        public IActionResult GetAllProducts()
        {
            return Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult GetProductsById(int id)
        {
            var foundProduct = products.FirstOrDefault(x => x.Id == id);
            if (foundProduct == null)
                return NotFound();
            return Ok(foundProduct);
        }

        [HttpGet]
        [Route("CheaperProducts")]
        public IActionResult GetCheaperProducts()
        {
            var filteredProducts = products.Where(x => x.Price < 3);
            if (filteredProducts == null || !filteredProducts.Any())
                return NotFound();
            return Ok(filteredProducts);
        }

        [HttpPost]
        public IActionResult AddNewProduct([FromBody] Product product)
        {
            if (product == null || product.Name == "" || product.Price <= 0)
            {
                return BadRequest();//400
            }


            products.ToList().Add(product);
            product.Id = products.Count() + 1;
            
            return CreatedAtAction(nameof(GetProductsById), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, [FromBody] Product product)
        {
            if (id != product.Id)
                return BadRequest();

            if (product.Name == "" || product.Price <= 0)
                return BadRequest();

            var productToUpdate = products.FirstOrDefault(x => x.Id == id);
            if (productToUpdate == null)
                return NotFound();

            productToUpdate.Name = product.Name;
            productToUpdate.Price = product.Price;
            productToUpdate.Description = product.Description;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var list = products.ToList();
            var productToDelete = list.FirstOrDefault(x => x.Id == id);
            if (productToDelete == null)
                return NotFound();

            
            list.Remove(productToDelete);
            products = list;
            return NoContent();
        }
    }
}

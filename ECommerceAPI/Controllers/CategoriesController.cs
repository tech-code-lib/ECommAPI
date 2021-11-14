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
    public class CategoriesController : ControllerBase
    {
        private readonly ECommDBContext _dbContext;
        private readonly IMapper _mapper;
        public CategoriesController(ECommDBContext context, IMapper mapper)
        {
            _dbContext = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAllCategories()
        {
            var categories = _dbContext.Categories;
            var categoriesDtos = categories.Select(x => _mapper.Map<Category, CategoryDTO>(x));
            return Ok(categoriesDtos);
        }

        [HttpGet("{id}")]
        public IActionResult GetCategoryById(int id)
        {
            var category = _dbContext.Categories.FirstOrDefault(x => x.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            var categoryDto = _mapper.Map<Category, CategoryDTO>(category);
            return Ok(categoryDto);
        }

        [HttpPost]
        public IActionResult AddACategory([FromBody] CategoryDTO category)
        {
            if (category == null || string.IsNullOrEmpty(category.Name))
            {
                return BadRequest();
            }

            var categoryToAdd = _mapper.Map<CategoryDTO, Category>(category);
            _dbContext.Categories.Add(categoryToAdd);
            _dbContext.SaveChanges();

            return CreatedAtAction(nameof(GetCategoryById), new { id = categoryToAdd.Id }, categoryToAdd);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateACategory(int id, [FromBody] CategoryDTO category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }

            var checkIfCategoryExist = _dbContext.Categories.Any(x => x.Id == category.Id);
            if (!checkIfCategoryExist)
            {
                return NotFound();
            }

            var categoryToUpdate = _mapper.Map<CategoryDTO, Category>(category);
            _dbContext.Entry(categoryToUpdate).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _dbContext.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteACategory(int id)
        {
            var categoryToDelete = _dbContext.Categories.FirstOrDefault(x => x.Id == id);
            if (categoryToDelete == null)
            {
                return NotFound();
            }
            _dbContext.Categories.Remove(categoryToDelete);
            _dbContext.SaveChanges();

            return NoContent();
        }

    }
}

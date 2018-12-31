using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using notes_api.DAL.Repositories;
using notes_api.Models.Domain;

namespace notes_api.Controllers
{
    [Route("api/[controller]")]
    public class CategoryController : Controller
    {
        public CategoryRepository _categories;
        public ItemRepository _items;

        public CategoryController(CategoryRepository categories, ItemRepository items)
        {
            _categories = categories;
            _items = items;
        }

        [HttpGet]
        public Task<IEnumerable<Category>> GetAll()
        {
            return _categories.GetAll();
        }

        [HttpGet("{id}", Name = "GetCategories")]
        public IActionResult GetById(Guid id)
        {
            var category = _categories.Get(id);
            if (category == null)
            {
                return NotFound();
            }
            return new ObjectResult(category);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Category category)
        {
            if (category == null)
                return BadRequest();

            category.CreatedAt = DateTime.UtcNow;
            _categories.Create(category);
            return new ObjectResult(category);
        }
        
        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] Category category)
        {
            if (category == null || category.Id != id)
                return BadRequest();

            _categories.Update(category);
            return new ObjectResult(category);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _items.DeleteByCategory(id);
            _categories.Delete(id);
            return new NoContentResult();
        }
    }
}

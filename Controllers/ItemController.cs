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
    public class ItemController : Controller
    {
        public ItemRepository _items;

        public ItemController(ItemRepository items)
        {
            _items = items;
        }

        [HttpGet]
        public Task<IEnumerable<Item>> GetAll()
        {
            return _items.GetAll();
        }

        [HttpGet("{id}", Name = "GetItems")]
        public IActionResult GetById(int id)
        {
            var item = _items.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Item item)
        {
            if (item == null)
                return BadRequest();

            item.CreatedAt = DateTime.UtcNow;
            _items.Create(item);
            return new ObjectResult(item);
        }
        
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Item item)
        {
            if (item == null || item.Id != id)
                return BadRequest();

            _items.Update(item);
            return new ObjectResult(item);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _items.Delete(id);
            return new NoContentResult();
        }
    }
}

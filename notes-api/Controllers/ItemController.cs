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

        [HttpGet]
        [Route("/api/item/category/{category_id}")]
        public Task<IEnumerable<Item>> GetByCategory(Guid category_id)
        {
            return _items.GetByCategory(category_id);
        }

        [HttpGet("{id}", Name = "GetItems")]
        public IActionResult GetById(Guid id)
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
        public IActionResult Update(Guid id, [FromBody] Item item)
        {
            if (item == null || item.Id != id)
                return BadRequest();

            _items.Update(item);
            return new ObjectResult(item);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _items.Delete(id);
            return new NoContentResult();
        }

        [HttpPut("/api/item/updateorder/{item_id}/{category_id}/{old_index}/{new_index}")]
        public IActionResult UpdateOrder(Guid item_id, Guid category_id, int old_index, int new_index)
        {
            _items.UpdateOrder(item_id, category_id, old_index, new_index);
            return Ok();
        }
    }
}

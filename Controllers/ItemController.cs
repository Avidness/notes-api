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
        public IItemRepository _items;

        public ItemController(IItemRepository items)
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
            Console.WriteLine(Request.Body);
            Console.WriteLine(Request.Headers);
            Console.WriteLine(Request.ContentType);
            Console.WriteLine(item);
            Console.WriteLine(item.Label);
            Console.WriteLine(item.Description);
            if (item == null)
                return BadRequest();

            item.CreatedAt = DateTime.UtcNow;
            _items.Create(item);
            return new ObjectResult(item.Id);
        }
        
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Item item)
        {
            if (item == null || item.Id != id)
                return BadRequest();

            _items.Update(item);
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _items.Delete(id);
            return new NoContentResult();
        }
    }
}

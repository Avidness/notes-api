using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using notes_api.Models.Domain;
using notes_api.DAL.EFCore;

namespace notes_api.DAL.Repositories
{
    public class ItemRepository : IRepository<Item>
    {
        private readonly MainContext _db;

        public ItemRepository(MainContext db)
        {
            _db = db;
        }
        
        public async void Create(Item item)
        {
            item.Id = Guid.NewGuid();
            item.LastModifiedAt = DateTime.UtcNow;
            item.CreatedAt = DateTime.UtcNow;
            
            var category = _db.Categories.Single(d => d.Id == item.Category.Id);
            item.Category = category;
            
            await _db.Items.AddAsync(item);
            _db.SaveChanges();
        }

        public void Update(Item item)
        {
            // TODO: use DTO's and automapping
            var existing_item = _db.Items
              .Where(x => x.Id == item.Id)
              .Include(x => x.Category)
              .SingleOrDefault();

            existing_item.Label = item.Label;
            existing_item.Description = item.Description;
            existing_item.LastModifiedAt = DateTime.UtcNow;

            if(existing_item.Category.Id != item.Category.Id){
                var newCat = _db.Categories.Single(d => d.Id == item.Category.Id);
                existing_item.Category = newCat;
            }

            _db.Items.Update(existing_item);
            _db.SaveChanges();
        }

        public async Task<IEnumerable<Item>> GetAll()
        {
            return await _db.Items
              .Include(x => x.Category)
              .ToListAsync();
        } 

        public async Task<IEnumerable<Item>> GetByCategory(Guid category_id)
        {
            return await _db.Items
              .Where(x => x.Category.Id == category_id)
              .Include(x => x.Category)
              .ToListAsync();
        } 

        public async Task<Item> Get(Guid id)
        {
            return await _db.Items
              .Include(x => x.Category)
              .SingleAsync(d => d.Id == id);
        } 

        public void Delete(Guid id)
        {
            var item = _db.Items.Find(id);
            _db.Items.Remove(item);
            _db.SaveChanges();
        }
    }
}
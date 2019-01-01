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
            item.Ordering = 0;
            
            var category = _db.Categories.Single(d => d.Id == item.Category.Id);
            item.Category = category;
            
            UpdateOrderRange(item.Category.Id, 0, GetLastOrdering(item.Category.Id), 1);
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
              .OrderBy(x => x.Ordering)
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

        public void DeleteByCategory(Guid category_id)
        {
            _db.Items
              .Where(x => x.Category.Id == category_id)
              .ToList()
              .ForEach(x => _db.Items.Remove(x));
            _db.SaveChanges();
        }

        public void UpdateOrder(Guid item_id, Guid category_id, int old_index, int new_index)
        {
            // Update the position of the moved item
            var item = _db.Items
                .Where(x => x.Id == item_id)
                .First();
            item.Ordering = new_index;
            _db.Items.Update(item);
            
            // Determine the how to shift the affected elements
            var shift_up = (old_index > new_index);
            var low = (shift_up ? new_index : old_index+1);
            var high = (shift_up ? old_index-1 : new_index);
            var increment = (shift_up ? 1 : -1);

            UpdateOrderRange(category_id, low, high, increment);
            _db.SaveChanges();
        }

        private int GetLastOrdering(Guid category_id){
            if(_db.Items.Count() == 0)
              return 0;
            
            return _db.Items
                .OrderByDescending(x => x.Ordering)
                .First()
                .Ordering;
        }

        private void UpdateOrderRange(Guid category_id, int low, int high, int increment)
        {
            var items = _db.Items
                .Where(x => 
                  x.Category.Id == category_id && 
                  x.Ordering >= low && 
                  x.Ordering <= high
                ).ToList();

            items.ForEach(x => x.Ordering = x.Ordering + increment);
        }
    }
}
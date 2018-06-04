using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using notes_api.Models.Domain;
using notes_api.DAL.EFCore;

namespace notes_api.DAL.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly MainContext _db;

        public ItemRepository(MainContext db)
        {
            _db = db;
        }
        
        public async void Create(Item item)
        {
            item.LastModifiedAt = DateTime.UtcNow;
            item.CreatedAt = DateTime.UtcNow;
            await _db.Items.AddAsync(item);
            _db.SaveChanges();
        }

        public void Update(Item item)
        {
            item.LastModifiedAt = DateTime.UtcNow;

            // TODO: use DTO's and automapping
            var existing_item = _db.Items.Find(item.Id);
            existing_item.Label = item.Label;
            existing_item.Description = item.Description;

            _db.Items.Update(existing_item);
            _db.SaveChanges();
        }

        public async Task<IEnumerable<Item>> GetAll()
        {
            return await _db.Items.ToListAsync();
        } 

        public async Task<Item> Get(int id)
        {
            return await _db.Items.SingleAsync(d => d.Id == id);
        } 

        public void Delete(int id)
        {
            var item = _db.Items.Find(id);
            _db.Items.Remove(item);
            _db.SaveChanges();
        }
    }
}
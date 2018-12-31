using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using notes_api.Models.Domain;
using notes_api.DAL.EFCore;

namespace notes_api.DAL.Repositories
{
    public class CategoryRepository : IRepository<Category>
    {
        private readonly MainContext _db;

        public CategoryRepository(MainContext db)
        {
            _db = db;
        }
        
        public async void Create(Category category)
        {
            category.Id = Guid.NewGuid();
            category.LastModifiedAt = DateTime.UtcNow;
            category.CreatedAt = DateTime.UtcNow;
            await _db.Categories.AddAsync(category);
            _db.SaveChanges();
        }

        public void Update(Category category)
        {
            var existing_category = _db.Categories.Find(category.Id);
            existing_category.Label = category.Label;
            existing_category.LastModifiedAt = DateTime.UtcNow;

            _db.Categories.Update(existing_category);
            _db.SaveChanges();
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            return await _db.Categories
              .OrderBy(x => x.CreatedAt)
              .ToListAsync();
        } 

        public async Task<Category> Get(Guid id)
        {
            return await _db.Categories.SingleAsync(d => d.Id == id);
        } 

        public void Delete(Guid id)
        {
            var category = _db.Categories.Find(id);
            _db.Categories.Remove(category);
            _db.SaveChanges();
        }
    }
}
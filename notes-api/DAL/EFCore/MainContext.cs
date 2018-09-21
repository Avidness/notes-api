using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using notes_api.Models.Domain;

namespace notes_api.DAL.EFCore
{
    public class MainContext : DbContext
    {
        public MainContext(DbContextOptions<MainContext> options) : base(options)
        { }

        public DbSet<Item> Items { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
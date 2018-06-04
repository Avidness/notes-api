using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using notes_api.Models.Domain;
using notes_api.DAL.EFCore;

namespace notes_api.DAL.Repositories
{
    public interface IItemRepository : IRepository<Item>
    {
    }
}
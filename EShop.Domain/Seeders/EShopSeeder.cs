using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EShop.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
namespace EShop.Domain.Seeders;
public class EShopSeeder : IEshopSeeder
{
    public readonly ApplicationDbContext _context;
    public EShopSeeder(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task Initialize()
    {
        _context.Database.EnsureCreated();
        if (_context.Products.Any()) return;
        var products = new List<Product>
        {
            new Product { Id = 1, Name = "Oleksandr", Ean = "43t79rh8fney78dj", Price = 50, Stock = 250, Sku = "ABC4545", Created_by = Guid.NewGuid(), Updated_by = Guid.NewGuid() },
            new Product { Id = 2, Name = "Bartek", Ean = "43t79ryrtgt4trtey78dj", Price = 40, Stock = 150, Sku = "ABCd45323245", Created_by = Guid.NewGuid(), Updated_by = Guid.NewGuid()},
            new Product { Id = 3, Name = "Piotr", Ean = "2443yrtvdffney78dj", Price = 100, Stock = 50, Sku = "DRFT754", Created_at = new DateTime(2024, 3, 23), Uploaded_at = new DateTime(2023, 5, 15) }
        };

        _context.Products.AddRange(products);
        await _context.SaveChangesAsync();
    }
}
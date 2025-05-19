using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Models;
using User.Domain.Repositories;

namespace User.Domain.Seeders
{
    public class UserSeeder : IUserSeeder
    {
        private readonly ApplicationDbContext _context;
        public UserSeeder(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Initialize()
        {
            _context.Database.EnsureCreated();
            if (_context.Users.Any()) return;

            List<UserDb> users = new List<UserDb> 
            { 
                new UserDb { Id = 123, Username = "admin", PasswordHash = Hasher.Hash("password"), Email = "admin@gmail.com" },
                new UserDb { Id = 456, Username = "user", PasswordHash = Hasher.Hash("sanya777"), Email = "sanya777@gmail.com" }
            };

            _context.Users.AddRange(users);
            await _context.SaveChangesAsync();
        }
    }
}

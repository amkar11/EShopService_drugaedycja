using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using User.Domain;
using User.Domain.Models;
using User.Domain.Repositories;

namespace User.Application
{
    public class UserDataDAO : IUserDataDAO
    {
        private ApplicationDbContext _context {  get; set; }
        public UserDataDAO(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<UserDb?> GetUserDataAsync(int userId)
        {
            UserDb? user = await _context.Users.SingleOrDefaultAsync(x => x.Id == userId);
            return user;
        }

        public async Task<UserDb?> GetUserDataAsync(string username, string password)
        {
            string password_hashed = Hasher.Hash(password);
            UserDb? user = await _context.Users.SingleOrDefaultAsync(x => x.Username == username && x.PasswordHash == password_hashed);
            return user;
        }
    }
}

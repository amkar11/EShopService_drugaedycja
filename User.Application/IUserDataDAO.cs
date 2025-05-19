using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Models;

namespace User.Application
{
    public interface IUserDataDAO
    {
        Task<UserDb?> GetUserDataAsync(int userId);
        Task<UserDb?> GetUserDataAsync(string username, string password);
    }
}

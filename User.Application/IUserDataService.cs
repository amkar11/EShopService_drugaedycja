using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Models;

namespace User.Application
{
    public interface IUserDataService
    {
        Task<UserDb?> GetUserDataAsync(int userId);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Repositories;

namespace User.Domain.Seeders
{
    public interface IUserSeeder
    {
        Task Initialize();
    }
}

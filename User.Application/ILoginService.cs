using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Application
{
    public interface ILoginService
    {
        string Login(string name, string password);
    }
}

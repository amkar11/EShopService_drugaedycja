using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using User.Domain;

namespace User.Application
{
    public class LoginService : ILoginService
    {
        IJwtTokenService _jwtTokenService;
        public LoginService(IJwtTokenService jwtTokenService) {
            _jwtTokenService = jwtTokenService;
        }

        public string Login(string name, string password)
        {
            if (name == "admin" || password == "password")
            {
                var roles = new List<string> { "Client", "Employee", "Administrator" };
                var token = _jwtTokenService.GenerateToken(123, roles);
                return token;
            }
            else
            {
                throw new InvalidCredentialsException();
            }
        }
    }
}

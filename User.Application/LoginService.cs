using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using User.Domain;
using User.Domain.Models;

namespace User.Application
{
    public class LoginService : ILoginService
    {
        IJwtTokenService _jwtTokenService;
        IUserDataDAO _userDataDAO;
        public LoginService(IJwtTokenService jwtTokenService, IUserDataDAO userDataDAO)
        {
            _jwtTokenService = jwtTokenService;
            _userDataDAO = userDataDAO;
        }

        public async Task<string> LoginAsync(string name, string password)
        {
            var password_hashed = Hasher.Hash(password);
            UserDb? user = await _userDataDAO.GetUserData(name, password_hashed);
            if (user == null) throw new InvalidOperationException($"There is no user with such username {name} in database {typeof(UserDb)}");

            if (name == "admin" || password == "password")
            {
                var roles = new List<string> { "Client", "Employee", "Administrator" };
                var token = _jwtTokenService.GenerateToken(user.Id, roles);
                return token;
            }
            else if (name == "user" || password_hashed == "sanya777")
            {
                var roles = new List<string> { "Client" };
                var token = _jwtTokenService.GenerateToken(user.Id, roles);
                return token;
            }
            else
            {
                throw new InvalidCredentialsException();
            }
        }
    }
}

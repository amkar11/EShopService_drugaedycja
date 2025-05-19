using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Models;

namespace User.Application
{
    public class UserDataService : IUserDataService
    {
        private IUserDataDAO _userDataDAO;
        public UserDataService(IUserDataDAO userDataDAO)
        {
            _userDataDAO = userDataDAO;
        }
        public Task<UserDb?> GetUserDataAsync(int userId)
        {
            var user = _userDataDAO.GetUserData(userId);
            if (user == null)
            {
                throw new InvalidOperationException($"There is no user with such id {userId} in databse {typeof(UserDb)}");
            }
            return user;
        }
    }
}

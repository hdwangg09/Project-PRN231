using BusinessObject.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.UserRepo
{
    public interface IUserRepository
    {
        User Login(User user);

        bool Register(User user);

        User GetUserByID(int id);

        bool UpdateUser(User user);

        User GetUserByEmail(string email);

        bool IsPasswordUser(int userID, string password);
        bool ChangePassword(User user);
        bool checkExistEmail(string email);
    }
}

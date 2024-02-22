using BusinessObject.Modals;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.UserRepo
{
    public class UserRepository : IUserRepository
	{
		public User GetUserByID(int id) => UserDAO.Instance.GetUserByID(id);

		public bool UpdateUser(User user) => UserDAO.Instance.UpdateUser(user);

		public User Login(User user) => UserDAO.Instance.Login(user);

		public bool Register(User user) => UserDAO.Instance.Register(user);

		public User GetUserByEmail(string email) => UserDAO.Instance.GetUserByEmail(email);

        public bool ChangePassword(User user) => UserDAO.Instance.ChangePassword(user);

        public bool checkExistEmail(string email) => UserDAO.Instance.isExEmail(email);

        public bool IsPasswordUser(int userID, string password) => UserDAO.Instance.IsPasswordUser(userID, password);
    }
}

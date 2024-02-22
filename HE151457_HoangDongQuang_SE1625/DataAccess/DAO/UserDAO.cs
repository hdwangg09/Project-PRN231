using BusinessObject.Modals;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
	public class UserDAO
	{
		private static UserDAO instance = null;
		private static readonly object instanceLock = new object();

		private UserDAO() { }

		public static UserDAO Instance
		{
			get
			{
				lock (instanceLock)
				{
					if (instance == null)
						instance = new UserDAO();
				}
				return instance;
			}
		}

		public User GetUserByID(int id)
		{
			try
			{
				Asm2Context context = new Asm2Context();
				User user = context.Users
					.Include(u => u.Role)
					.SingleOrDefault(u => u.UserId == id);
				return user;

			}
			catch (Exception ex)
			{
				return null;
			}
		}

		public bool UpdateUser(User user)
		{
			try
			{
				using (var context = new Asm2Context())
				{
					var existingUser = context.Users.SingleOrDefault(u => u.UserId == user.UserId);
					if (existingUser != null)
					{
						existingUser.FristName = user.FristName;
						existingUser.LastName = user.LastName;
						context.SaveChanges();
						return true;
					}
					else
					{
						return false;
					}
				}
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public User Login(User user)
		{
			try
			{
				using (var context = new Asm2Context())
				{
					User userExist = context.Users
						.Include(u => u.Role)
						.SingleOrDefault(u => u.EmailAddress == user.EmailAddress && u.Password == user.Password);
					return userExist != null ? userExist : null;
				}
			}
			catch (Exception ex)
			{
				return null;
			}
		}

		public bool Register(User user)
		{
			try
			{
				using (var context = new Asm2Context())
				{
					context.Users.Add(user);
					context.SaveChanges();
					return true;
				}
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public User GetUserByEmail(string email)
		{
			try
			{
				using (var context = new Asm2Context())
				{
					return context.Users.FirstOrDefault(x => x.EmailAddress.ToLower() == email.ToLower());
				}
			}
			catch (Exception ex)
			{
				return null;
			}
		}


        public bool isExEmail(string email)
        {
            try
            {
                Asm2Context context = new Asm2Context();
                User? user1 = context.Users.SingleOrDefault(e => e.EmailAddress == email);
				if(user1 == null)
				{
					return false;
				}
				else
				{
					return true;
				}

            }
            catch (Exception)
            {
                return false;
            }
        }
		public bool IsPasswordUser(int userID, string password)
		{
			try
			{
                Asm2Context context = new Asm2Context();
                User? userToChangePassword = context.Users.SingleOrDefault(e => e.UserId == userID);
				if(userToChangePassword.Password != password)
				{
					return false;
				}
				else
				{
					return true;
				}
            }
            catch	(Exception ex)
			{
				return false;
			}
		}
        public bool ChangePassword(User user)
        {
            try
            {
                Asm2Context context = new Asm2Context();
                User? userToChangePassword = context.Users.SingleOrDefault(e => e.UserId == user.UserId);
                if (userToChangePassword == null)
                {
                    return false;
                }
                else
                {
                    userToChangePassword.Password = user.Password;
					context.Users.Update(userToChangePassword);
					context.SaveChanges();
					return true;
                }

            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

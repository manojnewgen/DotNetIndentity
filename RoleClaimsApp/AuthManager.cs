using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoleClaimsApp
{
    public class AuthManager
    {
        private readonly List<User> users = new List<User>();
        private readonly TokenManager tokenManager = new TokenManager();

        //Implement Register and Login methods to manage users and assign tokens.
        public void Register()
        {
            Console.Write("Enter a username: ");
            string username = Console.ReadLine();
            Console.Write("Enter a password: ");
            string password = Console.ReadLine();

            if (users.Exists(u => u.UserName == username))
            {
                Console.WriteLine("Username already exists.");
                return;
            }

            users.Add(new User { UserName = username, Password = password });
            Console.WriteLine("Registration successful.");
        }

        public User Login(User user)
        {
            if (user.UserName == "TestUser" && user.Password == "TestPassword")
            {
                user.Token = new TokenManager().GenerateToken(userName: user.UserName);
                return user;
            }

            return null;
        }
        public User GetUserByToken(string token)
        {
            return users.Find(u => u.Token == token);
        }
    }
}
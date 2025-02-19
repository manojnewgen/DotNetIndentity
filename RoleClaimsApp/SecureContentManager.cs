using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoleClaimsApp
{
    public class SecureContentManager
    {
        // Implement a ValidateToken method to check tokens and allow or deny access to content.
        private readonly AuthManager authManager;

        public SecureContentManager(AuthManager authManager)
        {
            this.authManager = authManager;
        }

        public void AccessSecureContent()
        {
            Console.Write("Enter your token: ");
            string token = Console.ReadLine();

            var user = authManager.GetUserByToken(token);
            if (user != null)
            {
                Console.WriteLine($"Access granted to secure content for user: {user.Username}");
            }
            else
            {
                Console.WriteLine("Access denied. Invalid token.");
            }
        }
    }
}
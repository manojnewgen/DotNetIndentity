using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoleClaimsApp
{
    public class TokenManager
    {

        //Implement a GenerateToken method to create a base64-encoded string as a token.
        public string GenerateToken(string userName)
        {
            var expiray = DateTime.UtcNow.AddMinutes(30).ToString();
            string ToenData = $"{userName}:{expiray}";
            var token = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(ToenData));
            return token;
        }


    }
}
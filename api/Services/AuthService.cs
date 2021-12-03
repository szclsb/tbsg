using server.Models;

namespace server.Services
{
    public class AuthService
    {
        public string HashPassword(string password)
        {
            // generates and appends salt internally
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string passwordHash, string passwordClaim)
        {
            // handles salt internally
            return BCrypt.Net.BCrypt.Verify(passwordClaim, passwordHash);
        }
        
        public string CreateJwt(User user)
        {
            return "jwt";
        }

        public bool VerifyJwt(string jwt)
        {
            return false;
        }
    }
}
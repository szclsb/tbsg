using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using server.Models;

namespace server.Services
{
    public class AuthService
    {
        private readonly JwtSecurityTokenHandler _tokenHandler;
        private readonly SigningCredentials _credentials;
        private readonly string _issuer;

        public AuthService(IConfiguration config)
        {
            _tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["jwt:secret"]));    
            _credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            _issuer = config["jwt:issuer"];
        }
        
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
            var token = new JwtSecurityToken(_issuer,    
                _issuer,    
                new []
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                    new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("roles", string.Join(",", user.Roles))
                },    
                expires: DateTime.Now.AddMinutes(120),    
                signingCredentials: _credentials);
            return _tokenHandler.WriteToken(token);
        }

        public bool VerifyJwt(string jwt)
        {
            return false;
        }
    }
}
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using api.Models;

namespace api.Services
{
    public class AuthService
    {
        private readonly JwtSecurityTokenHandler _tokenHandler;
        public SymmetricSecurityKey Key {get;}
        public string Issuer {get;}
        public string Audience {get;}

        public AuthService(IConfiguration config)
        {
            _tokenHandler = new JwtSecurityTokenHandler();
            Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["jwt:secret"]));
            Issuer = config["jwt:issuer"];
            Audience = config["jwt:issuer"];
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
            var credentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new (JwtRegisteredClaimNames.Sub, user.Id),
                new (JwtRegisteredClaimNames.UniqueName, user.Username),
                new (JwtRegisteredClaimNames.Email, user.Email)
            };
            //claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role)));
            claims.AddRange(user.Roles.Select(role => new Claim("role", role)));
            var token = new JwtSecurityToken(
                issuer: Issuer,    
                audience: Audience,
                claims: claims, 
                expires: DateTime.Now.AddMinutes(120),    
                signingCredentials: credentials);
            return _tokenHandler.WriteToken(token);
        }
    }
}
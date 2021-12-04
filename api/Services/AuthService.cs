using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using server.Models;

namespace server.Services
{
    public class AuthService
    {
        private readonly JwtSecurityTokenHandler _tokenHandler;
        private readonly SymmetricSecurityKey _key;
        private readonly string _issuer;
        private readonly string _audience;

        public AuthService(IConfiguration config)
        {
            _tokenHandler = new JwtSecurityTokenHandler();
            _key = GetKey(config);    
            _issuer = GetIssuer(config);
            _audience = GetAudience(config);
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
            var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new (JwtRegisteredClaimNames.Sub, user.Id),
                new (ClaimTypes.Name, user.Username),
                new (ClaimTypes.Email, user.Email)
            };
            claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role)));
            var token = new JwtSecurityToken(
                issuer: _issuer,    
                audience: _audience,
                claims: claims, 
                expires: DateTime.Now.AddMinutes(120),    
                signingCredentials: credentials);
            return _tokenHandler.WriteToken(token);
        }

        public static SymmetricSecurityKey GetKey(IConfiguration config) =>
            new (Encoding.UTF8.GetBytes(config["jwt:secret"]));

        public static string GetIssuer(IConfiguration config) => config["jwt:issuer"];
        public static string GetAudience(IConfiguration config) => config["jwt:issuer"];
    }
}
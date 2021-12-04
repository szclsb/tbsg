using api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace api
{
    public class ConfigureJwtBearerOptions : IPostConfigureOptions<JwtBearerOptions>
    {
        private readonly AuthService _authService;

        public ConfigureJwtBearerOptions(AuthService authService)
        {
            _authService = authService;
        }
        
        public void PostConfigure(string name, JwtBearerOptions options)
        {
            // options.Events = new JwtBearerEvents
            // {
            //     OnTokenValidated = context =>
            //     {
            //         Console.Out.WriteLine(context.Principal);
            //         return null;
            //     }
            // };
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateActor = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _authService.Issuer,
                ValidAudience = _authService.Audience,
                IssuerSigningKey = _authService.Key,
                //NameClaimType = ClaimTypes.NameIdentifier
            };
        }
    }
}
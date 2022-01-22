using System.Collections.Generic;
using System.Threading.Tasks;
using api.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using api.Models;
using api.Services;

namespace api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IUserService _userService;
        private readonly AuthService _authService;
        
        public AuthController(IUserService userService, AuthService authService, ILogger<AuthController> logger)
        {
            _logger = logger;
            _userService = userService;
            _authService = authService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> Register(Registration registration)
        {
            //TODO verify user input
            var hash = _authService.HashPassword(registration.Password);
            var user = new User
            {
                Username = registration.Username,
                Email = registration.Email,
                Password = hash,
                Roles = new List<string>()
            };
            try
            {
                await _userService.Create(user);
                return Ok();
            }
            catch (DoublicateException _)
            {
                return Conflict();
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<string>> Login(Credentials credentials)
        {
            //TODO verify user input
            var user = await _userService.FindByUsername(credentials.Username);
            if (user == null)
            {
                return BadRequest();
            }

            return _authService.VerifyPassword(user.Password, credentials.Password) ?
                Ok(_authService.CreateJwt(user)) : BadRequest();
        }
    }
}
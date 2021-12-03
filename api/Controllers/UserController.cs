using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using server.Models;
using server.Services;

namespace server.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserService _userService;
        
        public UserController(UserService service, ILogger<UserController> logger)
        {
            _logger = logger;
            _userService = service;
        }

        [HttpPost]
        public async Task<ActionResult<string>> Create(User user)
        {
            await _userService.Create(user);
            _logger.LogInformation($"Created user {user.Username} with id {user.Id}");
            return CreatedAtRoute("GetUser", new { id = user.Id }, user);
        }
        
        [HttpGet("{id}", Name = "GetUser")]
        public async Task<ActionResult<User>> Find(string id)
        {
            var user = await _userService.Get(id);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> FindAll()
        {
            return await _userService.Get();
        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult<User>> Update(string id, User user)
        {
            var dbUser = await _userService.Get(id);
            if (dbUser == null)
            {
                return NotFound();
            }
            await _userService.Update(id, user);
            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> Delete(string id)
        {
            var user = await _userService.Get(id);
            if (user == null)
            {
                return NotFound();
            }
            await _userService.Remove(user.Id);
            return NoContent();
        }
    }
}
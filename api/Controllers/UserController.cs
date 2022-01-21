using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using api.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using api.Models;
using api.Services;

namespace api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(IUserService service, ILogger<UserController> logger)
        {
            _logger = logger;
            _userService = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            try
            {
                await _userService.Create(user);
                _logger.LogInformation($"Created user {user.Username} with id {user.Id}");
                return CreatedAtRoute("FindUser", new { id = user.Id }, user);
            }
            catch (DoublicateException de)
            {
                return Conflict();
            }
        }

        [HttpGet("{id}", Name = "FindUser")]
        public async Task<IActionResult> Find(string id)
        {
            try
            {
                var user = await _userService.Find(id);
                return user == null ? NotFound() : Ok(user);
            }
            catch (DoublicateException de)
            {
                return Conflict();
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> FindAll()
        {
            return Ok(await _userService.FindMany());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, User user)
        {
            try
            {
                var dbUser = await _userService.Find(id);
                if (dbUser == null)
                {
                    return NotFound();
                }

                await _userService.Update(id, user);
                return NoContent();
            }
            catch (DoublicateException de)
            {
                return Conflict();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> Delete(string id)
        {
            try
            {
                var user = await _userService.Find(id);
                if (user == null)
                {
                    return NotFound();
                }
                await _userService.Delete(user.Id);
                return NoContent();
            }
            catch (DoublicateException de)
            {
                return Conflict();
            }
        }
    }
}
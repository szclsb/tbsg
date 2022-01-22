using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using api.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize]
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
        [Authorize(Roles = Role.Admin)]
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
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            try
            {
                if (userId == id || HttpContext.User.IsInRole(Role.Admin))
                {
                    var user = await _userService.Find(id);
                    return user == null ? NotFound() : Ok(user);
                }

                return Forbid();
            }
            catch (DoublicateException de)
            {
                return Conflict();
            }
        }

        [HttpGet]
        [Authorize(Roles = Role.Admin)]
        public async Task<ActionResult<IEnumerable<User>>> FindAll()
        {
            return Ok(await _userService.FindMany());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, User user)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            try
            {
                if (userId == id || HttpContext.User.IsInRole(Role.Admin))
                {
                    var dbUser = await _userService.Find(id);
                    if (dbUser == null)
                    {
                        return NotFound();
                    }

                    await _userService.Update(id, user);
                    return NoContent();
                }

                return Forbid();
            }
            catch (DoublicateException de)
            {
                return Conflict();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> Delete(string id)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            try
            {
                if (userId == id || HttpContext.User.IsInRole(Role.Admin))
                {
                    var user = await _userService.Find(id);
                    if (user == null)
                    {
                        return NotFound();
                    }

                    await _userService.Delete(user.Id);
                    return NoContent();
                }

                return Forbid();
            }
            catch (DoublicateException de)
            {
                return Conflict();
            }
        }
    }
}
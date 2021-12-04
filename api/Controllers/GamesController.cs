using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using api.Models;

namespace api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/games")]
    public class GamesController : ControllerBase
    {
        private static readonly string[] Games = new[]
        {
            "Reversi"
        };

        private readonly ILogger<GamesController> _logger;

        public GamesController(ILogger<GamesController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return Games;
        }
        
        [Authorize(Roles = Role.Admin)]
        [HttpGet]
        [Route("admin")]
        public ActionResult<string> GetAdmin()
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return $"admin: {userId}";
        }
        
        [Authorize(Roles = Role.Caster)]
        [HttpGet]
        [Route("caster")]
        public ActionResult<string> GetCaster()
        {
            return "caster";
        }
    }
}
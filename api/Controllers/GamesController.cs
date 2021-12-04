using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public ActionResult<IEnumerable<string>> GetAdmin()
        {
            return new List<string>
            {
                "admin"
            };
        }
        
        [Authorize(Roles = Role.Caster)]
        [HttpGet]
        [Route("caster")]
        public ActionResult<IEnumerable<string>> GetCaster()
        {
            return new List<string>
            {
                "caster"
            };
        }
    }
}
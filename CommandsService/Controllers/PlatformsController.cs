using AutoMapper;
using CommandsService.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CommandsService.Controllers
{
    [Route("api/commandsservice")]
    [ApiController]
    public class PlatformsController : Controller
    {
        private readonly ICommandService _commandService;
        private readonly IMapper _mapper;
        private readonly ILogger<PlatformsController> _logger;

        public PlatformsController(ICommandService commandService, IMapper mapper, ILogger<PlatformsController> logger)
        {
            _commandService = commandService;
            _mapper = mapper;
            _logger = logger;
        }
        [HttpGet("Platforms/GetAllPlatforms")]
        public async Task<IActionResult> GetAllPlatforms()
        {
            var platforms = await _commandService.GetAllPlatforms();
            _logger.LogInformation("GetAllPlatforms in commandsservice called with response {platforms}", JsonConvert.SerializeObject(platforms));
            return Ok(platforms);
        }
        [HttpPost("Platforms/Index")]
        public IActionResult Index()
        {
            _logger.LogInformation("Commands Service get called :D");

            return Ok("Hello");
        }
    }
}

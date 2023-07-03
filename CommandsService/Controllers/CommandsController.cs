using CommandsService.Data.Dto;
using CommandsService.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CommandsService.Controllers
{
    [Route("api/commandsservice/{platformId}")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandService _commandService;
        private readonly ILogger<CommandsController> _logger;

        public CommandsController(ICommandService commandService, ILogger<CommandsController> logger)
        {
            _commandService = commandService;
            _logger = logger;
        }
        [HttpGet("Commands/GetCommandsForPlatform")]
        public async Task<IActionResult> GetCommandsForPlatform(Guid platformId)
        {
            var commands = await _commandService.GetCommandsForPlatform(platformId);

            _logger.LogInformation("GetCommandsForPlatform in commandsservice called with response: {commands}", JsonConvert.SerializeObject(commands));

            return commands == null ? NotFound("Platform not existed") : Ok(commands);
        }

        [HttpGet("Commands/GetCommandForPlatform/{commandId}")]
        public async Task<IActionResult> GetCommandForPlatform(Guid platformId, Guid commandId)
        {
            var command = await _commandService.GetCommandForPlatform(platformId, commandId);

            _logger.LogInformation("GetCommandForPlatform in commandsservice called with response: {command}", JsonConvert.SerializeObject(command));

            return command == null ? NotFound("Platform not existed") : Ok(command);
        }

        [HttpPost("Commands/AddCommandForPlatform")]
        public async Task<IActionResult> AddCommandForPlatform(Guid platformId, AddCommandDto addCommandDto)
        {
            if (addCommandDto == null || platformId.Equals(Guid.Empty))
            {
                _logger.LogError("Command data is invalid with Data: {commandDto} and PlatformId {platformId}", JsonConvert.SerializeObject(addCommandDto), platformId);
                return BadRequest("Command data is invalid");
            }

            var response = await _commandService.AddCommand(platformId, addCommandDto);

            return response.command == null ? NotFound("Platform not existed") :
                CreatedAtAction(nameof(GetCommandForPlatform), new { platformId = platformId, commandId = response.commandId }, addCommandDto);
        }
    }
}

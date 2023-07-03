using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PlatformService.DAL.Async;
using PlatformService.DAL.Sync;
using PlatformService.Data.Dto;
using PlatformService.Data.Interfaces;

namespace PlatformService.Controllers
{
    [Route("api/platformservice")]
    [ApiController]
    public class PlaformController : Controller
    {
        private readonly IPlatformService _platformService;
        private readonly ILogger<PlaformController> _logger;
        private readonly IHttpCommandsService _httpCommandsService;
        private readonly IMessageBusService _messageBusService;
        private readonly IMapper _mapper;

        public PlaformController(IPlatformService platformService, ILogger<PlaformController> logger
            , IHttpCommandsService httpCommandsService, IMessageBusService messageBusService,
            IMapper mapper)
        {
            _platformService = platformService;
            _logger = logger;
            _httpCommandsService = httpCommandsService;
            _messageBusService = messageBusService;
            _mapper = mapper;
        }

        [HttpGet("Platform/GetAllPlatforms")]
        public async Task<IActionResult> GetAllPlatforms()
        {
            var platforms = await _platformService.GetAllPlatforms();
            _logger.LogInformation("GetAllPlatforms in platformservice called with response {platforms}", JsonConvert.SerializeObject(platforms));
            return Ok(platforms);
        }

        [HttpGet("Platform/GetPlatformById/{id}")]
        public async Task<IActionResult> GetPlatformById(Guid id)
        {
            var platform = await _platformService.GetPlatformById(id);
            _logger.LogInformation("GetPlatformById in platformservice with ID: {id} called with response {platform}",
                id.ToString(), JsonConvert.SerializeObject(platform));
            return platform is null ? NotFound() : Ok(platform);
        }

        [HttpPost("Platform/AddPlatform")]
        public async Task<IActionResult> AddPlatform(AddPlatformDto addPlatformDto)
        {
            if (addPlatformDto == null)
                return BadRequest();

            var addedPlatform = await _platformService.AddPlatform(addPlatformDto);

            try
            {
                await _httpCommandsService.TestCommandsServiceConnection(addedPlatform);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception calling Commands Service with message: {ex.Message}");
            }

            //making Asyncronous call
            try
            {
                var platformPublishedDto = _mapper.Map<PlatformPublishedDto>(addedPlatform);
                platformPublishedDto.Event = "Publish_Platform";
                _messageBusService.PublishNewPlatform(platformPublishedDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception publishing message bus with message: {ex.Message}");
            }
            return CreatedAtAction(nameof(GetPlatformById), new { id = addedPlatform.Id }, addedPlatform);
        }
    }
}

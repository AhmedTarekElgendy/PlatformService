using AutoMapper;
using CommandsService.Data.Dto;
using CommandsService.Data.Interfaces;
using CommandsService.Models;
using Newtonsoft.Json;

namespace CommandsService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly ILogger<EventProcessor> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IMapper _mapper;
        private Platform platform;
        public EventProcessor(ILogger<EventProcessor> logger, IServiceScopeFactory serviceScopeFactory,
            IMapper mapper)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            _mapper = mapper;
        }
        public async void ProcessEvent(string message)
        {
            var platformDto = JsonConvert.DeserializeObject<PlatformPublishedDto>(message);
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var _commandService = scope.ServiceProvider.GetRequiredService<ICommandService>();
                if (platformDto is not null)
                {
                    platform = _mapper.Map<Platform>(platformDto);
                    if (platform is not null)
                        if (!await _commandService.IsPlatformExists(platformDto.Id))
                            await _commandService.AddPlatform(platform);
                }
            }
        }
        private EventType GetEventType(string message)
        {
            var eventName = JsonConvert.DeserializeObject<GenericEventDto>(message);

            switch (eventName.Event)
            {
                case "Publish_Platform":
                    _logger.LogInformation($"Event type is PublishedEvent");
                    return EventType.PublishedEvent;
                default:
                    _logger.LogInformation($"Event type is Undefined");
                    return EventType.Undefined;
            }
        }
        enum EventType
        {
            PublishedEvent,
            Undefined
        }
    }
}

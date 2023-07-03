using AutoMapper;
using CommandsService.Data.Dto;
using CommandsService.Data.Interfaces;
using CommandsService.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CommandsService.Data.Services
{
    public class CommandService : ICommandService
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<CommandService> _logger;
        private readonly IMapper _mapper;

        public CommandService(AppDbContext dbContext, ILogger<CommandService> logger, IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<(CommandDataDto?, Guid)> AddCommand(Guid platformId, AddCommandDto commandDto)
        {
            if (!await PlatformExists(platformId))
            {
                _logger.LogWarning("Platform with ID: {platformId} not existed", platformId);
                return (null, Guid.Empty);
            }

            var addedCommand = await _dbContext.Commands.AddAsync(_mapper.Map<Command>(commandDto));

            if (await SaveChanges() == true)
                _logger.LogInformation("Command added successfully with data: {commandDto}", JsonConvert.SerializeObject(addedCommand));
            else
            {
                _logger.LogError("Error with saving data in DB");
            }

            return (_mapper.Map<CommandDataDto>(addedCommand), addedCommand.Entity.Id);
        }

        public async Task<PlatformDataDto> AddPlatform(Platform platform)
        {
            var addedPlatform = await _dbContext.Platforms.AddAsync(platform);
            var isSaved = await SaveChanges();

            _logger.LogInformation($"Added Platform {addedPlatform} and saved changes is {isSaved}",
                JsonConvert.SerializeObject(addedPlatform.Entity), isSaved);

            return _mapper.Map<PlatformDataDto>(addedPlatform.Entity);
        }

        public async Task<List<PlatformDataDto>> GetAllPlatforms() =>

            _mapper.Map<List<PlatformDataDto>>(await _dbContext.Platforms.ToListAsync());


        public async Task<CommandDataDto?> GetCommandForPlatform(Guid platformId, Guid commandId)
        {
            if (await PlatformExists(platformId))
                return _mapper.Map<CommandDataDto>(await _dbContext.Commands.Include(c => c.Platform).FirstOrDefaultAsync(c => c.PlatformId == platformId && c.Id == commandId));

            _logger.LogWarning("Platform with ID: {platformId} has no commands", platformId);
            return null;
        }

        public async Task<List<CommandDataDto>?> GetCommandsForPlatform(Guid platformId)
        {
            if (await PlatformExists(platformId))
                return _mapper.Map<List<CommandDataDto>>(await _dbContext.Commands.Where(c => c.PlatformId == platformId).ToListAsync());

            _logger.LogWarning("Platform with ID: {platformId} has no commands", platformId);
            return null;
        }

        public async Task<bool> IsPlatformExists(Guid externalPlatformId) =>
           await _dbContext.Platforms.AnyAsync(p => p.ExternalId == externalPlatformId);


        public async Task<bool> PlatformExists(Guid platformId)
        {
            var platformExists = await _dbContext.Platforms.AnyAsync(p => p.Id == platformId);
            if (platformExists)
                _logger.LogInformation("Platform with ID: {platformId} exists", platformId);
            else
                _logger.LogWarning("Platform with ID: {platformId} not existed", platformId);

            return platformExists;
        }

        public async Task<bool> SaveChanges() =>

            await _dbContext.SaveChangesAsync() >= 0;
    }
}

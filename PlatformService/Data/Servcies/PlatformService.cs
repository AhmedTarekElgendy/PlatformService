using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PlatformService.Data.Dto;
using PlatformService.Data.Interfaces;
using PlatformService.Models;

namespace PlatformService.Data.Servcies
{
    public class PlatformService : IPlatformService
    {
        private readonly PlatformDBContext _platformDBContext;
        private readonly IMapper _mapper;
        private readonly ILogger<PlatformService> _logger;

        public PlatformService(PlatformDBContext platformDBContext, IMapper mapper, ILogger<PlatformService> logger)
        {
            _platformDBContext = platformDBContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PlatformDataDto> AddPlatform(AddPlatformDto platformDto)
        {
            if (platformDto == null)
                return null;

            _logger.LogInformation("Request payload for AddPlatform {platformDto}", JsonConvert.SerializeObject(platformDto));
            var platform = _mapper.Map<Platform>(platformDto);
            var addedPlatform = await _platformDBContext.Platforms.AddAsync(platform);

            var isSaved = await SaveChanges();

            _logger.LogInformation("Added Platform {addedPlatform} and saved changes is {isSaved}",
                JsonConvert.SerializeObject(addedPlatform.Entity), isSaved);

            return _mapper.Map<PlatformDataDto>(addedPlatform.Entity);
        }

        public async Task<bool> DeletePlatform(Guid Id)
        {
            var platform = await _platformDBContext.Platforms.FirstOrDefaultAsync(x => x.Id == Id);

            if (platform == null) return false;

            _logger.LogInformation("Request payload for DeletePlatform {platform}", JsonConvert.SerializeObject(platform));

            _platformDBContext.Platforms.Remove(platform);

            var isSaved = await SaveChanges();

            _logger.LogInformation("Request payload for DeletePlatform {platform} and saved changes is {isSaved}",
                JsonConvert.SerializeObject(platform), isSaved);

            return isSaved;
        }

        public async Task<List<PlatformDataDto>> GetAllPlatforms() =>
            _mapper.Map<List<PlatformDataDto>>(await _platformDBContext.Platforms.ToListAsync());


        public async Task<PlatformDataDto> GetPlatformById(Guid Id) =>
           _mapper.Map<PlatformDataDto>(await _platformDBContext.Platforms.FirstOrDefaultAsync(p => p.Id == Id));

        public async Task<bool> SaveChanges() =>
            await _platformDBContext.SaveChangesAsync() >= 0;

    }
}

using CommandsService.Data.Dto;
using CommandsService.Models;

namespace CommandsService.Data.Interfaces
{
    public interface ICommandService
    {
        Task<bool> SaveChanges();
        //Platforms
        Task<List<PlatformDataDto>> GetAllPlatforms();
        Task<PlatformDataDto> AddPlatform(Platform platform);
        Task<bool> IsPlatformExists(Guid externalPlatformId);
        //Commmands

        Task<(CommandDataDto? command, Guid commandId)> AddCommand(Guid platformId, AddCommandDto commandDto);
        Task<CommandDataDto?> GetCommandForPlatform(Guid platformId, Guid commandId);
        Task<List<CommandDataDto>?> GetCommandsForPlatform(Guid platformId);
    }
}

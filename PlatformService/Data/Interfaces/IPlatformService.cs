using PlatformService.Data.Dto;

namespace PlatformService.Data.Interfaces
{
    public interface IPlatformService
    {
        Task<List<PlatformDataDto>> GetAllPlatforms();

        Task<PlatformDataDto> AddPlatform(AddPlatformDto platform);

        Task<PlatformDataDto> GetPlatformById(Guid Id);

        Task<bool> DeletePlatform(Guid Id);

        Task<bool> SaveChanges();
    }
}

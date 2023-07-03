using PlatformService.Data.Dto;

namespace PlatformService.DAL.Sync
{
    public interface IHttpCommandsService
    {
        Task TestCommandsServiceConnection(PlatformDataDto platformDataDto);
    }
}

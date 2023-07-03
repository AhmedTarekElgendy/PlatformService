using PlatformService.Data.Dto;

namespace PlatformService.DAL.Async
{
    public interface IMessageBusService
    {
        void PublishNewPlatform(PlatformPublishedDto platoformPublishedDto);
    }
}

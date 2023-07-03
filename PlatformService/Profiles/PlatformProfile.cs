using AutoMapper;
using PlatformService.Data.Dto;
using PlatformService.Models;

namespace PlatformService.Profiles
{
    public class PlatformProfile : Profile
    {
        public PlatformProfile()
        {
            CreateMap<Platform, PlatformDataDto>().ReverseMap();
            CreateMap<AddPlatformDto, Platform>().ReverseMap();
            CreateMap<PlatformDataDto, PlatformPublishedDto>().ReverseMap();
        }
    }
}

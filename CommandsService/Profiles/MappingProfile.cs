using AutoMapper;
using CommandsService.Data.Dto;
using CommandsService.Models;

namespace CommandsService.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Platform, PlatformDataDto>();
            CreateMap<Command, CommandDataDto>().ForMember(c => c.PlatformName, s => s.MapFrom(s => s.Platform.Name));
            CreateMap<AddCommandDto, Command>();
            CreateMap<PlatformPublishedDto, Platform>().ForMember(dest => dest.ExternalId, op => op.MapFrom(sor => sor.Id));
        }
    }
}

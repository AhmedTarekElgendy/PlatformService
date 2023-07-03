using System.ComponentModel.DataAnnotations;

namespace CommandsService.Data.Dto
{
    public class CommandDataDto
    {
        public string HowTo { get; set; }

        public string CommandLine { get; set; }

        public string PlatformName { get; set; }
    }
}

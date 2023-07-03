using System.ComponentModel.DataAnnotations;

namespace CommandsService.Data.Dto
{
    public class PlatformDataDto
    {
        public Guid ExternalId { get; set; }

        public string Name { get; set; }
    }
}

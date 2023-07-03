using System.ComponentModel.DataAnnotations;

namespace CommandsService.Data.Dto
{
    public class AddCommandDto
    {
        [Required]
        public string HowTo { get; set; }

        [Required]
        public string CommandLine { get; set; }

        [Required]
        public Guid PlatformId { get; set; }
    }
}

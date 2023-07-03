using System.ComponentModel.DataAnnotations;

namespace PlatformService.Data.Dto
{
    public class AddPlatformDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Publisher { get; set; }
        [Required]
        public string Cost { get; set; }
    }
}

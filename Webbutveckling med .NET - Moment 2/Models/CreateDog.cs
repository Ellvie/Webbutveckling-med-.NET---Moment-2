using System.ComponentModel.DataAnnotations;

namespace Webbutveckling_med_.NET___Moment_2.Models
{
    public class CreateDog
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Breed { get; set; }

        [Required]
        public string Age { get; set; }

        [Required]
        public string Gender { get; set; }
        public IFormFile Pic { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace ITRockChallenge.DTOs
{
    public class CreateTaskDto
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(300)]
        public string Description { get; set; } = string.Empty;
    }
}

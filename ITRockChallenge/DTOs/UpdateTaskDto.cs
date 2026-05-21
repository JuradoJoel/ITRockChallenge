using System.ComponentModel.DataAnnotations;

namespace ITRockChallenge.DTOs
{
    public class UpdateTaskDto
    {
        [StringLength(100)]
        public string? Title { get; set; } = string.Empty;
        [StringLength(300)]
        public string? Description { get; set; } = string.Empty;
        public bool? Completed { get; set; }
    }
}

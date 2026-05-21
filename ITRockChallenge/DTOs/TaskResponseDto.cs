namespace ITRockChallenge.DTOs
{
    public class TaskResponseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool Completed { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

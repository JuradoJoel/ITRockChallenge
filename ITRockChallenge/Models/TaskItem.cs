namespace ITRockChallenge.Models
{
    public class TaskItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool Completed { get; set; } = false;
        public string UserId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}

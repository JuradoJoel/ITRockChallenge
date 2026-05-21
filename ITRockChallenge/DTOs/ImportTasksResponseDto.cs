namespace ITRockChallenge.DTOs
{
    public class ImportTasksResponseDto
    {
        public int ImportedCount { get; set; }

        public List<TaskResponseDto> Tasks { get; set; } = new();
    }
}
using System.Text.Json;
using ITRockChallenge.DTOs;


namespace ITRockChallenge.Services
{
    public class TaskImportService
    {
        private readonly HttpClient _httpClient;

        public TaskImportService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ExternalTodoDto>> ImportTodosAsync()
        {
            var response = await _httpClient.GetAsync(
                "https://jsonplaceholder.typicode.com/todos");

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var todos = JsonSerializer.Deserialize<List<ExternalTodoDto>>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                });

            return todos?
                .Where(t => t.UserId == 1)
                .Take(5)
                .ToList()
                ?? new List<ExternalTodoDto>();
        }
    }
}

using ITRockChallenge.Data;
using ITRockChallenge.DTOs;
using ITRockChallenge.Models;
using ITRockChallenge.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ITRockChallenge.Controllers
{
    [ApiController]
    [Route("tasks")]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private readonly TaskImportService _taskImportService;
        public TasksController(ApplicationDbContext context, TaskImportService taskImportService)
        {
            _context = context;
            _taskImportService = taskImportService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<TaskResponseDto>>> GetTasks()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var tasks = await _context.Tasks
                .Where(t => t.UserId == userId)
                .Select(t => new TaskResponseDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    Completed = t.Completed,
                    CreatedAt = t.CreatedAt,
                })
                .ToListAsync();
            return Ok(tasks);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<TaskResponseDto>> CreateTask(CreateTaskDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var task = new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Description = dto.Description,
                Completed = false,
                UserId = userId!,
                CreatedAt = DateTime.UtcNow
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            var response = new TaskResponseDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Completed = task.Completed,
                CreatedAt = task.CreatedAt
            };
            return CreatedAtAction(nameof(GetTasks), new {id = task.Id}, response);
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TaskResponseDto>> UpdateTask(Guid id, UpdateTaskDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }
            if (task.UserId != userId)
            {
                return Forbid();
            }
            if (dto.Title is not null)
            {
                task.Title = dto.Title;
            }
            if (dto.Description is not null)
            {
                task.Description = dto.Description;
            }
            if (dto.Completed.HasValue)
            {
                task.Completed = dto.Completed.Value;
            }

            await _context.SaveChangesAsync();
            var response = new TaskResponseDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Completed = task.Completed,
                CreatedAt = task.CreatedAt
            };

            return Ok(response);
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            if (task.UserId != userId)
            {
                return Forbid();
            }

            _context.Tasks.Remove(task);

            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpPost("import")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ImportTasksResponseDto>> ImportTasks()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var externalTasks = await _taskImportService.ImportTodosAsync();

            var tasks = externalTasks.Select(t => new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = t.Title,
                Description = "Importada desde la API externa",
                Completed = t.Completed,
                UserId = userId!,
                CreatedAt = DateTime.UtcNow,
            }).ToList();

            _context.Tasks.AddRange(tasks);

            await _context.SaveChangesAsync();

            var response = new ImportTasksResponseDto
            {
                ImportedCount = tasks.Count,
                Tasks = tasks.Select(t => new TaskResponseDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    Completed = t.Completed,
                    CreatedAt = t.CreatedAt,
                }).ToList()
            };

            return Ok(response);
        }
    }
}

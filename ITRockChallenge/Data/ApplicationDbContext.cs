using ITRockChallenge.Models;
using Microsoft.EntityFrameworkCore;

namespace ITRockChallenge.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<TaskItem> Tasks { get; set; }
    }
}
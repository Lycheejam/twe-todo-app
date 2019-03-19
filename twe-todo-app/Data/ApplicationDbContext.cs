using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using twe_todo_app.Models.TodoModels;

namespace twe_todo_app.Data {
    public class ApplicationDbContext : IdentityDbContext {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) {
        }

        public DbSet<Todo> Todos { get; set; }
        public DbSet<Item> Items { get; set; }
    }
}

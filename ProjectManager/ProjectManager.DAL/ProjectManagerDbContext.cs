using ProjectManager.Entity;
using System.Data.Entity;

namespace ProjectManager.DAL
{
    public class ProjectManagerDbContext : DbContext
    {
        public ProjectManagerDbContext() : base("name=ProjectManagerConnection") { }

        public DbSet<Users> Users { get; set; }

        public DbSet<Projects> Projects { get; set; }

        public DbSet<Tasks> Tasks { get; set; }
    }
}

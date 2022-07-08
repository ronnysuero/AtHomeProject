using AtHomeProject.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AtHomeProject.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Users> Users { get; set; }
        public DbSet<UsersClaims> UsersClaims { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
    }
}

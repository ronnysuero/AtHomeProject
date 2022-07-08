using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace AtHomeProject.Data.Tests
{
    [ExcludeFromCodeCoverage]
    public class SharedDatabaseFixture
    {
        public ApplicationDbContext CreateContext()
        {
            var applicationDbContext = new ApplicationDbContext(
                new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseSqlite($"Filename=${Guid.NewGuid()}.db")
                    .Options
            );

            applicationDbContext.Database.EnsureDeleted();
            applicationDbContext.Database.EnsureCreated();

            return applicationDbContext;
        }
    }
}

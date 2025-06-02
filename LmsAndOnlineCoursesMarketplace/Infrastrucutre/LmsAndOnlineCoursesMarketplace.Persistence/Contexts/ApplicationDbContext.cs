using LmsAndOnlineCoursesMarketplace.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LmsAndOnlineCoursesMarketplace.Persistence.Contexts;

public class ApplicationDbContext: IdentityDbContext
{
    public DbSet<Course> Courses { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
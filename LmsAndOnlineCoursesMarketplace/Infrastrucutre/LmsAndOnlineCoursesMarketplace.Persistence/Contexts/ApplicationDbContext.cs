using LmsAndOnlineCoursesMarketplace.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LmsAndOnlineCoursesMarketplace.Persistence.Contexts;

public class ApplicationDbContext: IdentityDbContext
{
    public DbSet<Course> Courses { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserCoursePurchase> UserCoursePurchases { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Course>()
            .HasOne(c => c.User)
            .WithMany(u => u.Courses)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<UserCoursePurchase>()
            .HasKey(up => new { up.UserId, up.CourseId });

        modelBuilder.Entity<UserCoursePurchase>()
            .HasOne(up => up.User)
            .WithMany(u => u.PurchasedCourses)
            .HasForeignKey(up => up.UserId);

        modelBuilder.Entity<UserCoursePurchase>()
            .HasOne(up => up.Course)
            .WithMany(c => c.UserCoursePurchases)
            .HasForeignKey(up => up.CourseId);
    }
}
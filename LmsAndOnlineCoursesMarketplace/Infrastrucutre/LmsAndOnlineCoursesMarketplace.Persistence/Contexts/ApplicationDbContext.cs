using LmsAndOnlineCoursesMarketplace.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LmsAndOnlineCoursesMarketplace.Persistence.Contexts;

public class ApplicationDbContext: IdentityDbContext
{
    public DbSet<Course> Courses { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserCoursePurchase> UserCoursePurchases { get; set; }
    public DbSet<UserSubscription> UserSubscriptions { get; set; }
    public DbSet<CourseReaction> CourseReactions { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }
    public DbSet<LiveStream> LiveStreams { get; set; }
    public DbSet<LiveStreamReactions> LiveStreamReactions { get; set; }
    
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
        
        modelBuilder.Entity<UserSubscription>()
            .HasKey(us => new { us.SubscriberId, us.SubscribedToId });

        modelBuilder.Entity<UserSubscription>()
            .HasOne(us => us.Subscriber)
            .WithMany(u => u.Subscriptions)
            .HasForeignKey(us => us.SubscriberId);

        modelBuilder.Entity<UserSubscription>()
            .HasOne(us => us.SubscribedTo)
            .WithMany(u => u.Subscribers)
            .HasForeignKey(us => us.SubscribedToId);
        
        modelBuilder.Entity<CourseReaction>()
            .HasKey(cr => new { cr.CourseId, cr.UserId });

        modelBuilder.Entity<CourseReaction>()
            .HasOne(cr => cr.Course)
            .WithMany(c => c.Reactions)
            .HasForeignKey(cr => cr.CourseId);

        modelBuilder.Entity<CourseReaction>()
            .HasOne(cr => cr.User)
            .WithMany(u => u.CourseReactions)
            .HasForeignKey(cr => cr.UserId);
        
        modelBuilder.Entity<ChatMessage>()
            .HasOne(m => m.Sender)
            .WithMany(u => u.SentMessages)
            .HasForeignKey(m => m.SenderId);

        modelBuilder.Entity<ChatMessage>()
            .HasOne(m => m.Recipient)
            .WithMany(u => u.ReceivedMessages)
            .HasForeignKey(m => m.RecipientId);
        
        modelBuilder.Entity<LiveStreamReactions>()
            .HasKey(r => new { r.StreamId, r.UserId });
        
        modelBuilder.Entity<LiveStreamReactions>()
            .HasOne<User>(r => r.User)
            .WithMany(u => u.LiveStreamReactions)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade); // или.Restrict

        modelBuilder.Entity<LiveStreamReactions>()
            .HasOne<LiveStream>(r => r.LiveStream)
            .WithMany(s => s.Reactions)
            .HasForeignKey(r => r.StreamId);
        
    }
}
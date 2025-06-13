using LmsAndOnlineCoursesMarketplace.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace LmsAndOnlineCoursesMarketplace.Domain.Entities;

public class User : BaseAuditableEntity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string JobPosition { get; set; }
    public string Description { get; set; }
    public int EnrollStudents { get; set; }
    public int CoursesCnt { get; set; }
    public int ReviewsCnt { get; set; }
    public int SubscriptionsCnt { get; set; }
    public decimal Balance { get; set; } = 350.0m;
    public string? IdentityUserId { get; set; }
    public virtual IdentityUser? IdentityUser { get; set; }
    
    public virtual ICollection<ChatMessage> SentMessages { get; set; } = new List<ChatMessage>();
    public virtual ICollection<ChatMessage> ReceivedMessages { get; set; } = new List<ChatMessage>();
    public virtual ICollection<Course> Courses { get; set; }
    public virtual ICollection<CourseReaction> CourseReactions { get; set; } = new HashSet<CourseReaction>();
    public virtual ICollection<LiveStreamReactions> LiveStreamReactions { get; set; } = new HashSet<LiveStreamReactions>();
    public virtual ICollection<UserCoursePurchase> PurchasedCourses { get; set; }
    public virtual ICollection<UserSubscription> Subscribers { get; set; }
    public virtual ICollection<UserSubscription> Subscriptions { get; set; }
}
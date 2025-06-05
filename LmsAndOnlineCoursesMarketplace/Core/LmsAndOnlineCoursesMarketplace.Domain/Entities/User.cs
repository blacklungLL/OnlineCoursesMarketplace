using LmsAndOnlineCoursesMarketplace.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace LmsAndOnlineCoursesMarketplace.Domain.Entities;

public class User : BaseAuditableEntity
{
    public string Name { get; set; }
    public string Email { get; set; }

    public string JobPosition { get; set; }
    public int EnrollStudents { get; set; }
    public int CoursesCnt { get; set; }
    public int ReviewsCnt { get; set; }
    public int SubscriptionsCnt { get; set; }

    public string? IdentityUserId { get; set; } // Ссылка на IdentityUser
    public virtual IdentityUser? IdentityUser { get; set; }

    public virtual ICollection<Course> Courses { get; set; }
}
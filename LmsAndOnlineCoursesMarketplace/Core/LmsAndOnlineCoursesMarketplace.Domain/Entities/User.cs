using LmsAndOnlineCoursesMarketplace.Domain.Common;

namespace LmsAndOnlineCoursesMarketplace.Domain.Entities;

public class User : BaseAuditableEntity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string JobPosition { get; set; }
    public int EnrollStudents { get; set; }
    public int CoursesCnt { get; set; }
    public int ReviewsCnt { get; set; }
    public int SubscriptionsCnt { get; set; }
    
    
}
using LmsAndOnlineCoursesMarketplace.Domain.Entities;

namespace LmsAndOnlineCoursesMarketplace.MVC.Models.Profile;

public class ProfileVM
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string JobPosition { get; set; }
    public int EnrollStudents { get; set; }
    public int CoursesCnt { get; set; }
    public int ReviewsCnt { get; set; }
    public int SubscriptionsCnt { get; set; }
}
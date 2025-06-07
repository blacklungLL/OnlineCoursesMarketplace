using LmsAndOnlineCoursesMarketplace.Domain.Entities;
using LmsAndOnlineCoursesMarketplace.MVC.Models.Course;
using LmsAndOnlineCoursesMarketplace.MVC.Models.OtherUserProfile;
using LmsAndOnlineCoursesMarketplace.MVC.Models.ShoppingCart;

namespace LmsAndOnlineCoursesMarketplace.MVC.Models.Profile;

public class ProfileVM
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string JobPosition { get; set; }
    public string Description { get; set; }
    public int EnrollStudents { get; set; }
    public int CoursesCnt { get; set; }
    public int ReviewsCnt { get; set; }
    public int SubscriptionsCnt { get; set; }
    public bool IsSubscribed { get; set; }
    public int CurrentUserId { get; set; } = -1;
    public List<CourseVM> CreatedCourses { get; set; } = new();
    public List<CourseSummaryVM> PurchasedCourses { get; set; } = new();
    public List<SubscriptionPreviewVM> Subscriptions { get; set; } = new();
}
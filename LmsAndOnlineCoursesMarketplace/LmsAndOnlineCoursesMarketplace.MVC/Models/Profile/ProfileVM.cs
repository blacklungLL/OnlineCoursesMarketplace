using LmsAndOnlineCoursesMarketplace.Domain.Entities;
using LmsAndOnlineCoursesMarketplace.MVC.Models.Course;
using LmsAndOnlineCoursesMarketplace.MVC.Models.ShoppingCart;

namespace LmsAndOnlineCoursesMarketplace.MVC.Models.Profile;

public class ProfileVM
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string JobPosition { get; set; }
    public string Description { get; set; }
    public int EnrollStudents { get; set; }
    public int CoursesCnt { get; set; }
    public int ReviewsCnt { get; set; }
    public int SubscriptionsCnt { get; set; }
    public List<CourseVM> CreatedCourses { get; set; } = new();
    public List<CourseSummaryVM> PurchasedCourses { get; set; } = new();
}
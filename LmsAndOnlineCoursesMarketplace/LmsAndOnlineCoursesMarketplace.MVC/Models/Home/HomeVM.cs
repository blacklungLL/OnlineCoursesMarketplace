using LmsAndOnlineCoursesMarketplace.Application.Features.Courses.Queries;

namespace LmsAndOnlineCoursesMarketplace.MVC.Models.Home;

public class HomeVM
{
    public IEnumerable<GetByCourseIdDto> FeaturedCourses { get; set; }
    public IEnumerable<GetByCourseIdDto> AllCourses { get; set; }
}
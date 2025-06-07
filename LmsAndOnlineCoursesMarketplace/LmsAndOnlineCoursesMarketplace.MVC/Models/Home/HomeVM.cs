using LmsAndOnlineCoursesMarketplace.Application.Features.Courses.Queries;
using LmsAndOnlineCoursesMarketplace.MVC.Models.Profile;

namespace LmsAndOnlineCoursesMarketplace.MVC.Models.Home;

public class HomeVM
{
    public IEnumerable<GetByCourseIdDto> FeaturedCourses { get; set; }
    public IEnumerable<GetByCourseIdDto> AllCourses { get; set; }
}
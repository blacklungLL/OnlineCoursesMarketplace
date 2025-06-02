using MediatR;

namespace LmsAndOnlineCoursesMarketplace.Application.Features.Courses.Queries
{
    public record GetByCourseIdQuery(int CourseId) : IRequest<IEnumerable<GetByCourseIdDto>>;
}
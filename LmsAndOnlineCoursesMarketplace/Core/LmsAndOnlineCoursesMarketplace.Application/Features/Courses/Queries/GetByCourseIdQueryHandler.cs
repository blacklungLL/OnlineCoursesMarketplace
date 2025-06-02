using MediatR;
using LmsAndOnlineCoursesMarketplace.Application.Interfaces.Repositories;

namespace LmsAndOnlineCoursesMarketplace.Application.Features.Courses.Queries
{
    internal class GetByCourseIdQueryHandler : IRequestHandler<GetByCourseIdQuery, IEnumerable<GetByCourseIdDto>>
    {
        private readonly ICourseRepository _courseRepository;

        public GetByCourseIdQueryHandler(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<IEnumerable<GetByCourseIdDto>> Handle(GetByCourseIdQuery query, CancellationToken cancellationToken)
        {
            if (query.CourseId == 0)
            {
                var allCourses = await _courseRepository.GetAllAsync();
                return allCourses.Select(course => new GetByCourseIdDto
                {
                    Id = course.Id,
                    Title = course.Title,
                    ShortDescription = course.ShortDescription,
                    UserId = course.UserId,
                    ImageLink = course.ImageLink,
                    Rating = course.Rating,
                    RatingsCnt = course.RatingsCnt,
                    Language = course.Language,
                    LastUpdate = course.LastUpdate,
                    Views = course.Views,
                    LikesCnt = course.LikesCnt,
                    DislikesCnt = course.DislikesCnt,
                    SharedCnt = course.SharedCnt,
                    Requirements = course.Requirements,
                    Description = course.Description,
                    Duration = course.Duration,
                    Category = course.Category,
                    Price = course.Price
                }).ToList();
            }

            var courses = await _courseRepository.GetByIdAsync(query.CourseId);

            var result = courses.Select(course => new GetByCourseIdDto
            {
                Id = course.Id,
                Title = course.Title,
                ShortDescription = course.ShortDescription,
                UserId = course.UserId,
                ImageLink = course.ImageLink,
                Rating = course.Rating,
                RatingsCnt = course.RatingsCnt,
                Language = course.Language,
                LastUpdate = course.LastUpdate,
                Views = course.Views,
                LikesCnt = course.LikesCnt,
                DislikesCnt = course.DislikesCnt,
                SharedCnt = course.SharedCnt,
                Requirements = course.Requirements,
                Description = course.Description,
                Duration = course.Duration,
                Category = course.Category,
                Price = course.Price
            });

            return result ?? new List<GetByCourseIdDto>();
        }
    }
}
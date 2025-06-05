using LmsAndOnlineCoursesMarketplace.Application.Features.Courses.Queries;
using LmsAndOnlineCoursesMarketplace.MVC.Models.Course;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LmsAndOnlineCoursesMarketplace.MVC.Controllers;

public class CourseController : Controller
{
    private readonly IMediator _mediator;

    public CourseController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IActionResult> Index(int id)
    {
        var query = new GetByCourseIdQuery(id);
        var courses = await _mediator.Send(query);
        var course = courses.FirstOrDefault();
        
        if (course == null)
            return NotFound();

        var viewModel = new CourseVM
        {
            Id = course.Id,
            Title = course.Title,
            ShortDescription = course.ShortDescription,
            UserId = course.UserId,
            AuthorName = course.AuthorName ?? "Unknown",
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
        };

        return View(viewModel);
    }
}
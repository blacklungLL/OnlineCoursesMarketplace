using LmsAndOnlineCoursesMarketplace.Application.Interfaces.Repositories;
using LmsAndOnlineCoursesMarketplace.Domain.Entities;
using LmsAndOnlineCoursesMarketplace.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace LmsAndOnlineCoursesMarketplace.Persistence.Repositories;

public class CourseRepository: ICourseRepository
{
    private readonly IGenericRepository<Course> _repository;
    private readonly ApplicationDbContext _context;

    public CourseRepository(IGenericRepository<Course> repository, ApplicationDbContext context)
    {
        _repository = repository;
        _context = context;
    }

    public async Task<IEnumerable<Course>> GetAllAsync()
    {
        return await _context.Courses
            .Include(c => c.User)
            .ToListAsync();
    }

    public async Task<IEnumerable<Course>> GetByIdAsync(int id)
    {
        return await _context.Courses
            .Include(c => c.User)
            .Where(c => c.Id == id)
            .ToListAsync();
    }
}
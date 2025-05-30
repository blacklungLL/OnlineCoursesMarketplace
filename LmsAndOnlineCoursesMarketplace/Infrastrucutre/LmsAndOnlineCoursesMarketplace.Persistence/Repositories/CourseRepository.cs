using LmsAndOnlineCoursesMarketplace.Application.Interfaces.Repositories;
using LmsAndOnlineCoursesMarketplace.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LmsAndOnlineCoursesMarketplace.Persistence.Repositories;

public class CourseRepository: ICourseRepository
{
    private readonly IGenericRepository<Course> _repository;

    public CourseRepository(IGenericRepository<Course> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Course>> GetAllAsync()
    {
        return await _repository.Entities.ToListAsync();
    }

    public async Task<IEnumerable<Course>> GetByIdAsync(int id)
    {
        return await _repository.Entities.Where(c => c.Id == id).ToListAsync();
    }
}
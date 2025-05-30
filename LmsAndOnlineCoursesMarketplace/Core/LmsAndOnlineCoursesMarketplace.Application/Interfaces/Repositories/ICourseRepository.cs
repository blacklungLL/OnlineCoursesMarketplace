using LmsAndOnlineCoursesMarketplace.Domain.Entities;

namespace LmsAndOnlineCoursesMarketplace.Application.Interfaces.Repositories;

public interface ICourseRepository
{
    Task<IEnumerable<Course>> GetByIdAsync(int id);
    
    Task<IEnumerable<Course>> GetAllAsync();
}
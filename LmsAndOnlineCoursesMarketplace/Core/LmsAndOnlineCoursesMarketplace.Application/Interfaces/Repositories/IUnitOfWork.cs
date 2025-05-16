using LmsAndOnlineCoursesMarketplace.Domain.Common;

namespace LmsAndOnlineCoursesMarketplace.Application.Interfaces.Repositories;

public interface IUnitOfWork //: IDisposable
{
    IGenericRepository<T> Repository<T>() where T : BaseAuditableEntity;
    Task<int> Save(CancellationToken cancellationToken);
    Task Rollback();
}
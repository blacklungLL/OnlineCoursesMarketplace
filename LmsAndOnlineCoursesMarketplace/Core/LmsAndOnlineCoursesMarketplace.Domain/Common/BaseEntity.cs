using LmsAndOnlineCoursesMarketplace.Domain.Common.Interfaces;

namespace LmsAndOnlineCoursesMarketplace.Domain.Common;

public abstract class BaseEntity : IEntity
{
    /// <summary>
    /// Уникальный индентификатор поста на странице пользователя
    /// </summary>
    public int Id { get; set; }
}
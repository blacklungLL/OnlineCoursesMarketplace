using Microsoft.EntityFrameworkCore;

namespace LmsAndOnlineCoursesMarketplace.Domain.Entities;

[Index(nameof(CourseId), nameof(UserId), IsUnique = true)]
public class CourseReaction
{
    public int CourseId { get; set; }
    public int UserId { get; set; }
    public bool IsLike { get; set; }
    public bool IsDislike { get; set; }

    public virtual Course Course { get; set; }
    public virtual User User { get; set; }
}
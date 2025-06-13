using LmsAndOnlineCoursesMarketplace.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace LmsAndOnlineCoursesMarketplace.Domain.Entities;

[Index(nameof(StreamId), nameof(UserId), IsUnique = true)]
public class LiveStreamReactions
{
    public int StreamId { get; set; }
    public int UserId { get; set; }
    public bool IsLike { get; set; }
    public bool IsDislike { get; set; }
    
    public virtual LiveStream LiveStream { get; set; }
    public virtual User User { get; set; }
}
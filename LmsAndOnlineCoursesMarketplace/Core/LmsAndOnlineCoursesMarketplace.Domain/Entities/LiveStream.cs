using LmsAndOnlineCoursesMarketplace.Domain.Common;

namespace LmsAndOnlineCoursesMarketplace.Domain.Entities;

public class LiveStream : BaseAuditableEntity
{
    public int UserId { get; set; }
    public string VideoLink { get; set; }
    public int Views { get; set; }
    public int LikesCnt { get; set; }
    public int DislikesCnt { get; set; }
    
    public virtual User User { get; set; }
    public virtual ICollection<LiveStreamReactions> Reactions { get; set; }
}
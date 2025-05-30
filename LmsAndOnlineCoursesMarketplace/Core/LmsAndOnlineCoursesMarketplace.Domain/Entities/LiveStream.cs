using LmsAndOnlineCoursesMarketplace.Domain.Common;

namespace LmsAndOnlineCoursesMarketplace.Domain.Entities;

public class LiveStream : BaseAuditableEntity
{
    public string Link { get; set; }
    public int UserId { get; set; }
    public int Views { get; set; }
    public int LikesCnt { get; set; }
    public int DislikesCnt { get; set; }
    public int SharedCnt { get; set; }
}
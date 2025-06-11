using LmsAndOnlineCoursesMarketplace.Domain.Common;

namespace LmsAndOnlineCoursesMarketplace.Domain.Entities;

public class Course: BaseAuditableEntity
{
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public int UserId { get; set; }
    public virtual User User { get; set; }
    public string ImageLink { get; set; }
    public decimal Rating { get; set; }
    public int RatingsCnt { get; set; }
    public string Language { get; set; }
    public int LastUpdate { get; set; }
    public int Views { get; set; }
    public int LikesCnt { get; set; }
    public int DislikesCnt { get; set; }
    public int SharedCnt { get; set; }
    public string Requirements { get; set; }
    public string Description { get; set; }
    public string Duration { get; set; }
    public string Category { get; set; }
    public decimal Price { get; set; }
    public virtual ICollection<CourseReaction> Reactions { get; set; }
    public virtual ICollection<UserCoursePurchase> UserCoursePurchases { get; set; }
}
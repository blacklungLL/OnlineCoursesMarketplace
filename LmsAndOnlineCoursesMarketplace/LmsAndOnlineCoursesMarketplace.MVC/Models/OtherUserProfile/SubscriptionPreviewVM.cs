namespace LmsAndOnlineCoursesMarketplace.MVC.Models.OtherUserProfile;

public class SubscriptionPreviewVM
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string JobPosition { get; set; } = string.Empty;
    public int SubscribersCnt { get; set; }
    public int CourseCnt { get; set; }
}
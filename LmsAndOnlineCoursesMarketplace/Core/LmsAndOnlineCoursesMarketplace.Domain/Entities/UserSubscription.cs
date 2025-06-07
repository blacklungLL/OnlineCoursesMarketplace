namespace LmsAndOnlineCoursesMarketplace.Domain.Entities;

public class UserSubscription
{
    public int SubscriberId { get; set; }
    public int SubscribedToId { get; set; }

    public virtual User Subscriber { get; set; }
    public virtual User SubscribedTo { get; set; }
}
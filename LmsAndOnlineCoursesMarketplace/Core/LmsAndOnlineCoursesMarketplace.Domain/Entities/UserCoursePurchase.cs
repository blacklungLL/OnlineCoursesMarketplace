namespace LmsAndOnlineCoursesMarketplace.Domain.Entities;

public class UserCoursePurchase
{
      public int UserId { get; set; }
      public int CourseId { get; set; }
      
      public virtual User User { get; set; }
      public virtual Course Course { get; set; }
}
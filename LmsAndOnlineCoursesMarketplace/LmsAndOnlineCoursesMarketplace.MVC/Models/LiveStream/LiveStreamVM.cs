namespace LmsAndOnlineCoursesMarketplace.MVC.Models.LiveStream;

public class LiveStreamVM
{
    public int Id {get; set;}
    public int UserId { get; set; }
    public string AuthorName { get; set; }
    public string? VideoSource { get; set; } 
    public string? EmbedUrl { get; set; }
    public int Views { get; set; }
    public int LikesCnt { get; set; }
    public int DislikesCnt { get; set; }
}
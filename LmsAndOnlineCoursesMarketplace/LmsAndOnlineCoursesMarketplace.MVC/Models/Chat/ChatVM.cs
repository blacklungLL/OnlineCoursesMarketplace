namespace LmsAndOnlineCoursesMarketplace.MVC.Models.Chat;

public class ChatVM
{
    public List<ChatUserVM> Users { get; set; } = new();
    public int? SelectedUserId { get; set; }
    public string? SelectedUserName { get; set; }
    public List<ChatMessageVM> Messages { get; set; } = new();
}

public class ChatUserVM
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string AvatarUrl { get; set; } = string.Empty;
}

public class ChatMessageVM
{
    public string Content { get; set; } = string.Empty;
    public string SenderName { get; set; } = string.Empty;
    public bool IsMine { get; set; }
    public string SentAt { get; set; } = string.Empty;
}
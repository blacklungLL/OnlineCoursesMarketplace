namespace LmsAndOnlineCoursesMarketplace.Domain.Entities;

public class ChatMessage
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public int SenderId { get; set; }
    public virtual User Sender { get; set; } = null!;
    public int RecipientId { get; set; }
    public virtual User Recipient { get; set; } = null!;
    public bool IsRead { get; set; }
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
}
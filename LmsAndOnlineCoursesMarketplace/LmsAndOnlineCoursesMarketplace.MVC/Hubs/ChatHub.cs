using LmsAndOnlineCoursesMarketplace.Domain.Entities;
using LmsAndOnlineCoursesMarketplace.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace LmsAndOnlineCoursesMarketplace.MVC.Hubs;

public class ChatHub : Hub
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public ChatHub(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // Подключаем пользователя к группе чата
    public async Task JoinAndLoad(int recipientId)
    {
        var identityUser = await _userManager.GetUserAsync(Context.User);
        var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.IdentityUserId == identityUser.Id);

        if (currentUser == null) return;

        string groupName = $"chat_{Math.Min(currentUser.Id, recipientId)}_{Math.Max(currentUser.Id, recipientId)}";

        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

        var messages = await _context.ChatMessages
            .Where(m =>
                (m.SenderId == currentUser.Id && m.RecipientId == recipientId) ||
                (m.SenderId == recipientId && m.RecipientId == currentUser.Id))
            .Include(m => m.Sender)
            .OrderBy(m => m.SentAt)
            .Select(m => new
            {
                content = m.Content,
                senderName = m.Sender.Name,
                isMine = m.SenderId == currentUser.Id,
                sentAt = m.SentAt.ToString("yyyy-MM-ddTHH:mm:ss")
            })
            .ToListAsync();

        await Clients.Client(Context.ConnectionId).SendAsync("LoadMessages", messages, recipientId);
    }

    // Отправка нового сообщения через группу
    public async Task SendPrivateMessage(string recipientId, string message)
    {
        var identityUser = await _userManager.GetUserAsync(Context.User);
        var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.IdentityUserId == identityUser.Id);

        if (currentUser == null || !int.TryParse(recipientId, out int recipientUserId)) return;

        var chatMessage = new ChatMessage
        {
            Content = message,
            SenderId = currentUser.Id,
            RecipientId = recipientUserId,
            SentAt = DateTime.UtcNow
        };

        string groupName = $"chat_{Math.Min(currentUser.Id, recipientUserId)}_{Math.Max(currentUser.Id, recipientUserId)}";

        Console.WriteLine($"Sending message to group: {groupName}");
        
        await Clients.Group($"chat_{Math.Min(currentUser.Id, recipientUserId)}_{Math.Max(currentUser.Id, recipientUserId)}")
            .SendAsync("ReceiveMessage", new
            {
                SenderId = currentUser.Id,
                Name = currentUser.Name,
                isMine = true,
            }, message, chatMessage.SentAt.ToString("yyyy-MM-ddTHH:mm:ss"));
        
        Console.WriteLine($"отправлено to group: {groupName}");
        await _context.ChatMessages.AddAsync(chatMessage);
        await _context.SaveChangesAsync();
    }
}
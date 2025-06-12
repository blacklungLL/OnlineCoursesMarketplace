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

    // Загрузка истории
    public async Task LoadChat(int recipientId)
    {
        var identityUser = await _userManager.GetUserAsync(Context.User);
        var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.IdentityUserId == identityUser.Id);

        if (currentUser == null) return;

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

    // Отправка нового сообщения
    public async Task SendPrivateMessage(string recipientId, string message)
    {
        var identityUser = await _userManager.GetUserAsync(Context.User);
        var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.IdentityUserId == identityUser.Id);

        if (currentUser == null) return;

        var chatMessage = new ChatMessage
        {
            Content = message,
            SenderId = currentUser.Id,
            RecipientId = int.Parse(recipientId),
            SentAt = DateTime.UtcNow
        };

        await _context.ChatMessages.AddAsync(chatMessage);
        await _context.SaveChangesAsync();

        // Отправляем себе и собеседнику
        await Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessage", new
        {
            Id = currentUser.Id,
            Name = currentUser.Name,
            isMine = true
        }, message, chatMessage.SentAt.ToString("yyyy-MM-ddTHH:mm:ss"));

        await Clients.User(recipientId).SendAsync("ReceiveMessage", new
        {
            Id = currentUser.Id,
            Name = currentUser.Name,
            isMine = false
        }, message, chatMessage.SentAt.ToString("yyyy-MM-ddTHH:mm:ss"));
    }
}
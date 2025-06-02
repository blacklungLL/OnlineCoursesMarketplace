using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using LmsAndOnlineCoursesMarketplace.Application.Interfaces.Services;

namespace LmsAndOnlineCoursesMarketplace.Infrastructure.Services;

public class SmtpEmailSender : IEmailSender
{
    private readonly string _smtpServer;
    private readonly int _smtpPort;
    private readonly string _smtpUsername;
    private readonly string _smtpPassword;

    public SmtpEmailSender(string smtpServer, int smtpPort, string smtpUsername, string smtpPassword)
    {
        _smtpServer = smtpServer;
        _smtpPort = smtpPort;
        _smtpUsername = smtpUsername;
        _smtpPassword = smtpPassword;
    }

    public async Task SendAsync(string email, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("LMS and Online courses Marketplace", _smtpUsername));
        message.To.Add(MailboxAddress.Parse(email));
        message.Subject = subject;

        var builder = new BodyBuilder
        {
            TextBody = body // Текст письма
        };

        message.Body = builder.ToMessageBody();

        using var client = new SmtpClient();
        await client.ConnectAsync(_smtpServer, _smtpPort, SecureSocketOptions.SslOnConnect);
        await client.AuthenticateAsync(_smtpUsername, _smtpPassword);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
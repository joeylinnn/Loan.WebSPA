using LoanServiceApp.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;


namespace LoanServiceApp.Services;

public class EmailSender : IEmailSender
{
    private readonly SmtpSettings _smtpSettings;
    private readonly ILogger<EmailSender> _logger;

    public EmailSender(IOptions<SmtpSettings> smtpOptions, ILogger<EmailSender> logger)
    {
        _smtpSettings = smtpOptions.Value;
        _logger = logger;
    }

    public async Task SendAsync(string to, string subject, string body)
    {
        var email = new MimeMessage();

        email.From.Add(new MailboxAddress(_smtpSettings.FromName, _smtpSettings.FromEmail));
        email.To.Add(MailboxAddress.Parse(to));

        if (!string.IsNullOrWhiteSpace(_smtpSettings.Bcc))
        {
            email.Bcc.Add(MailboxAddress.Parse(_smtpSettings.Bcc));
        }

        email.Subject = subject;

        email.Body = new TextPart("plain")
        {
            Text = body
        };

        using var smtp = new SmtpClient();

        // Demo/local convenience only. Remove if you do not need it.
        smtp.CheckCertificateRevocation = false;

        await smtp.ConnectAsync(
            _smtpSettings.Host,
            _smtpSettings.Port,
            SecureSocketOptions.StartTls);

        await smtp.AuthenticateAsync(
            _smtpSettings.UserName,
            _smtpSettings.Password);

        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);

        _logger.LogInformation("Email sent to {Recipient}", to);
    }
}
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace Library.Aplication.Services;

public interface IEmailService
{
    void SendEmail(string recipient, string subject, string body);
}

public sealed class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public void SendEmail(string recipient, string subject, string body)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(recipient);

        var settings = ReadSettings();
        try
        {
            using var client = new SmtpClient(settings.Server, settings.Port)
            {
                Credentials = new NetworkCredential(settings.Sender, settings.Password),
                EnableSsl = settings.EnableSsl,
                Timeout = settings.TimeoutMilliseconds
            };

            using var message = new MailMessage
            {
                From = new MailAddress(settings.Sender, settings.SenderName),
                Subject = subject ?? string.Empty,
                Body = body ?? string.Empty
            };
            message.To.Add(new MailAddress(recipient));
            client.Send(message);
        }
        catch (FormatException exception)
        {
            throw new InvalidOperationException("The sender or recipient email address is invalid.", exception);
        }
        catch (SmtpException exception)
        {
            throw new InvalidOperationException(
                $"SMTP delivery failed for {settings.Server}:{settings.Port} ({exception.StatusCode}). " +
                "Check the SMTP credentials, port, TLS setting, and provider account configuration.", exception);
        }
    }

    private SmtpSettings ReadSettings()
    {
        var server = Required("SmtpServer");
        var sender = Required("SenderEmail");
        var password = Required("AppPassword");

        if (server.Contains("gmail.com", StringComparison.OrdinalIgnoreCase))
        {
            password = string.Concat(password.Where(character => !char.IsWhiteSpace(character)));
        }

        return new SmtpSettings(
            server,
            GetInteger("Port", 587, 1, 65535),
            sender,
            password,
            _configuration["EmailSettings:SenderName"]?.Trim() ?? string.Empty,
            GetBoolean("EnableSsl", true),
            checked(GetInteger("TimeoutSeconds", 30, 1, 300) * 1000));
    }

    private string Required(string name)
    {
        var value = _configuration[$"EmailSettings:{name}"]?.Trim();
        return string.IsNullOrWhiteSpace(value)
            ? throw new InvalidOperationException($"Missing EmailSettings:{name} configuration.")
            : value;
    }

    private int GetInteger(string name, int defaultValue, int minimum, int maximum)
    {
        var value = _configuration[$"EmailSettings:{name}"];
        if (string.IsNullOrWhiteSpace(value)) return defaultValue;
        return int.TryParse(value, out var result) && result >= minimum && result <= maximum
            ? result
            : throw new InvalidOperationException($"EmailSettings:{name} must be between {minimum} and {maximum}.");
    }

    private bool GetBoolean(string name, bool defaultValue)
    {
        var value = _configuration[$"EmailSettings:{name}"];
        return string.IsNullOrWhiteSpace(value)
            ? defaultValue
            : bool.TryParse(value, out var result)
                ? result
                : throw new InvalidOperationException($"EmailSettings:{name} must be true or false.");
    }

    private sealed record SmtpSettings(
        string Server,
        int Port,
        string Sender,
        string Password,
        string SenderName,
        bool EnableSsl,
        int TimeoutMilliseconds);
}
using MailKit.Net.Smtp;
using MimeKit;
using MailKit;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using backend.DTOs;

namespace backend.Services
{
    public class EmailService
    {        
        private readonly string _smtpServer;
        private readonly int _port;
        private readonly string _fromEmail;
        private readonly string _password;

        public EmailService(IConfiguration configuration)
        {
            // Load SMTP settings from configuration
            _smtpServer = configuration["EmailSettings:SmtpServer"];
            _port = int.Parse(configuration["EmailSettings:Port"]);
            _fromEmail = configuration["EmailSettings:FromEmail"];
            _password = configuration["EmailSettings:Password"];
        }

        public async Task SendEmailAsync(EmailDTO emailDto)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("E-com", _fromEmail));
            message.To.Add(new MailboxAddress("", emailDto.ToEmail));
            message.Subject = emailDto.Subject;

            // Create the email body
            message.Body = new TextPart("html")
            {
                Text = emailDto.Body
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_smtpServer, _port, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_fromEmail, _password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true); 
            }
        }
    }
}
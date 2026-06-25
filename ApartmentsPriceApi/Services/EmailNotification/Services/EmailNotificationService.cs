using ApartmentsPriceApi.Services.EmailNotification.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace ApartmentsPriceApi.Services.EmailNotification.Services
{
    public class EmailNotificationService : IEmailNotificationService
    {
        private readonly SmtpOptions _options;

        public EmailNotificationService(IOptions<SmtpOptions> options)
        {
            _options = options.Value;
        }

        public async Task SendChangedPriceAsync(string email, string apartmentUrl, string oldPrice, string newPrice)
        {
            var message = new MimeMessage();

            message.From.Add(MailboxAddress.Parse(_options.From));
            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = "Цена квартиры изменилась";
            message.Body = new TextPart("plain")
            {
                Text = $"Цена квартиры по ссылке {apartmentUrl} изменилась с {oldPrice} руб. на {newPrice} руб."
            };

            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(
                _options.Host, 
                _options.Port, 
                SecureSocketOptions.StartTls);

            await smtp.AuthenticateAsync(
                _options.Username, 
                _options.Password);

            await smtp.SendAsync(message);

            await smtp.DisconnectAsync(true);
        }
    }
}

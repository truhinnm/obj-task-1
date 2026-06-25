namespace ApartmentsPriceApi.Services.EmailNotification.Interfaces
{
    public interface IEmailNotificationService
    {
        Task SendChangedPriceAsync(string email, string apartmentUrl, string oldPrice, string newPrice);
    }
}

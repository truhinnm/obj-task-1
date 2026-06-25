using ApartmentsPriceApi.Models;

namespace ApartmentsPriceApi.Services.SubscriptionService.Interfaces
{
    public interface ISubscriptionService
    {
        public Task SubscribeAsync(string apartmentUrl, string userEmail);

        public Task<List<Apartment>> GetAllSubscriptions();
    }
}

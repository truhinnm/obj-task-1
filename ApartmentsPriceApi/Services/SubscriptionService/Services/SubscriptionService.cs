using ApartmentsPriceApi.Data;
using ApartmentsPriceApi.Models;
using ApartmentsPriceApi.Services.ApartmentPrice.Interfaces;
using ApartmentsPriceApi.Services.SubscriptionService.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApartmentsPriceApi.Services.SubscriptionService.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ApartmentsDbContext _context;
        private readonly IApartmentPriceService _apartmentPriceService;

        public SubscriptionService(ApartmentsDbContext context, IApartmentPriceService apartmentPriceService)
        {
            _context = context;
            _apartmentPriceService = apartmentPriceService;
        }
        public async Task<List<Apartment>> GetAllSubscriptions()
        {
            return await _context.Apartments
                .Include(a => a.Subscriptions)
                .ThenInclude(s => s.Subscriber)
                .ToListAsync();
        }

        public async Task SubscribeAsync(string apartmentUrl, string userEmail)
        {

            var subscriber = await _context.Subscribers.FirstOrDefaultAsync(s => s.Email == userEmail);

            if (subscriber == null)
            {
                subscriber = new Subscriber { Email = userEmail };
                _context.Subscribers.Add(subscriber);
            }

            var apartment = await _context.Apartments.FirstOrDefaultAsync(a => a.Url == apartmentUrl);

            var price = await _apartmentPriceService.GetCurrentPriceAsync(apartmentUrl.Split('/', StringSplitOptions.RemoveEmptyEntries).Last());

            if (apartment == null)
            {
                apartment = new Apartment
                {
                    Url = apartmentUrl,
                    CurrentPrice = price
                };
                _context.Apartments.Add(apartment);
            }
            else
            {
                apartment.CurrentPrice = price;
            }

            await _context.SaveChangesAsync();

            var subscription = new Subscription
            {
                ApartmentId = apartment.Id,
                SubscriberId = subscriber.Id
            };

            _context.Subscriptions.Add(subscription);
            await _context.SaveChangesAsync();

        }
    }   
}

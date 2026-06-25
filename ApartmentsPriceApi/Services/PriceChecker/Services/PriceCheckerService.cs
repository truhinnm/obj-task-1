using ApartmentsPriceApi.Data;
using ApartmentsPriceApi.Services.ApartmentPrice.Interfaces;
using ApartmentsPriceApi.Services.EmailNotification.Interfaces;
using ApartmentsPriceApi.Services.PriceChecker.Interfaces;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace ApartmentsPriceApi.Services.PriceChecker.Services
{
    public class PriceCheckerService : IPriceCheckerService
    {
        private readonly ApartmentsDbContext _context;
        private readonly IApartmentPriceService _apartmentPriceService;
        private readonly IEmailNotificationService _emailNotificationService;

        public PriceCheckerService(
            ApartmentsDbContext context, 
            IApartmentPriceService apartmentPriceService, 
            IEmailNotificationService emailNotificationService)
        {
            _context = context;
            _apartmentPriceService = apartmentPriceService;
            _emailNotificationService = emailNotificationService;
        }

        public async Task CheckAsync()
        {
            var apartments = await _context.Apartments
                .Include(a => a.Subscriptions)
                .ThenInclude(s => s.Subscriber)
                .ToListAsync();

            foreach (var apartment in apartments)
            {
                var newPrice = await _apartmentPriceService.GetCurrentPriceAsync(apartment.Url.Split('/', StringSplitOptions.RemoveEmptyEntries).Last());

                if (newPrice != apartment.CurrentPrice)
                {
                    foreach(var sub in apartment.Subscriptions)
                    {
                        await _emailNotificationService.SendChangedPriceAsync(sub.Subscriber.Email, apartment.Url, apartment.CurrentPrice, newPrice);
                    }
                    apartment.CurrentPrice = newPrice;
                }

                await _context.SaveChangesAsync();
            }
        }
    }
}

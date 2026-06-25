using ApartmentsPriceApi.DTOs;
using ApartmentsPriceApi.Services.EmailNotification.Interfaces;
using ApartmentsPriceApi.Services.SubscriptionService.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentsPriceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;
        private readonly IEmailNotificationService _notificationService;

        public SubscriptionController(ISubscriptionService subscriptionService, IEmailNotificationService notificationService)
        {
            _subscriptionService = subscriptionService;
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSubscriptions()
        {
            var subscriptions = await _subscriptionService.GetAllSubscriptions();
            return Ok(subscriptions
                .Select(s => new 
                {
                    ApartmentUrl = s.Url,
                    ApartmentPrice = s.CurrentPrice
                })
                .ToList());
        }

        [HttpPost]
        public async Task<IActionResult> Subscribe([FromBody] SubscriptionRequestDTO request)
        {
            await _subscriptionService.SubscribeAsync(request.ApartmentUrl, request.UserEmail);
            return Ok("Subscription successful");
        }
    }
}

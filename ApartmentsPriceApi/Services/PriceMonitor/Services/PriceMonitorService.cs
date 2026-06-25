using ApartmentsPriceApi.Services.PriceChecker.Interfaces;

namespace ApartmentsPriceApi.Services.PriceMonitor.Services
{
    public class PriceMonitorService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public PriceMonitorService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceScopeFactory.CreateScope();

                var priceChecker = scope.ServiceProvider.GetRequiredService<IPriceCheckerService>();

                await priceChecker.CheckAsync();

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}

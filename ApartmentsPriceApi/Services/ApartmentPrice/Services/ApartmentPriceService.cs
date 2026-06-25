using ApartmentsPriceApi.Services.ApartmentPrice.Interfaces;
using System.Text.Json;

namespace ApartmentsPriceApi.Services.ApartmentPrice.Services
{
    public class ApartmentPriceService : IApartmentPriceService
    {
        const string API_URL = "https://prinzip.su/api/v1/public/apartments/";
        private readonly HttpClient _httpClient;

        public ApartmentPriceService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<string> GetCurrentPriceAsync(string apartmentId)
        {
            var response = await _httpClient.GetStringAsync($"{API_URL}{apartmentId}");
            
            using var document = JsonDocument.Parse(response);

            var root = document.RootElement;

            if (root.TryGetProperty("pricings", out var pricings) || pricings.GetArrayLength() != 0)
            {
                if (pricings[0].TryGetProperty("price_base", out var priceBase))
                {
                    return priceBase.GetString()!;
                }
                else
                {
                    return pricings[0].GetProperty("price").ToString()!;
                }
            }

            return "Price not found";

        }
    }
}

namespace ApartmentsPriceApi.Services.ApartmentPrice.Interfaces
{
    public interface IApartmentPriceService
    {
        Task<string> GetCurrentPriceAsync(string apartmentId);
    }
}

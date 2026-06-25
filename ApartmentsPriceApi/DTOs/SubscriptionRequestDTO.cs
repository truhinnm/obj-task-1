namespace ApartmentsPriceApi.DTOs
{
    public class SubscriptionRequestDTO
    {
        public required string ApartmentUrl { get; set; }
        public required string UserEmail { get; set; }
    }
}

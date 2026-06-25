using System.ComponentModel.DataAnnotations;

namespace ApartmentsPriceApi.Models
{
    public class Apartment
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public required string Url { get; set; }

        [Required]
        public required string CurrentPrice {  get; set; }
        
        public List<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    }

}

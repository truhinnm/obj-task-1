using System.ComponentModel.DataAnnotations;

namespace ApartmentsPriceApi.Models
{
    public class Subscription
    {
        [Required]
        public int ApartmentId { get; set; }

        public Apartment Apartment { get; set; } = null!;

        [Required]
        public int SubscriberId { get; set; }

        public Subscriber Subscriber { get; set; } = null!;
    }
}

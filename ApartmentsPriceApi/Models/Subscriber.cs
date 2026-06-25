using System.ComponentModel.DataAnnotations;

namespace ApartmentsPriceApi.Models
{
    public class Subscriber
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public required string Email { get; set; }

        public List<Subscription> Subscriptions { get; set; } = new List<Subscription>();

    }
}

using System.ComponentModel.DataAnnotations;

namespace TravelEaseBackend.Models
{
    public class Traveler
    {
        [Key]
        public string TravelerId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}

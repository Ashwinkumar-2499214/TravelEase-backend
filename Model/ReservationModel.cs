using System.ComponentModel.DataAnnotations; // Add this namespace
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelEase.Model
{
    public class ReservationModel
    {
        [Key] // Marks this as the Primary Key
        public int ReservationID { get; set; }
        public int BookingID { get; set; }
        public string? Details { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        [NotMapped]
        public object? Destination { get; set; }
    }
}
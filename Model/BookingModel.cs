using System.ComponentModel.DataAnnotations; // 1. Add this namespace

namespace TravelEase.Model
{
    public class BookingModel
    {
        [Key] // 2. Add this attribute
        public int BookingID { get; set; }
        public int CustomerID { get; set; }
        public int PartnerID { get; set; }
        public string? ItemType { get; set; }
        public DateTime Date { get; set; }
        public string? Status { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
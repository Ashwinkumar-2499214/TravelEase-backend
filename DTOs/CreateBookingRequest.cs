using System.ComponentModel.DataAnnotations;

namespace TravelEase.DTOs
{
    /// <summary>
    /// Request DTO for creating a new booking with optional reservations.
    /// Supports booking workflow initiation (checkout).
    /// </summary>
    public class CreateBookingRequest
    {
        [Required(ErrorMessage = "CustomerID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "CustomerID must be greater than 0")]
        public int CustomerID { get; set; }

        [Required(ErrorMessage = "PartnerID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "PartnerID must be greater than 0")]
        public int PartnerID { get; set; }

        [Required(ErrorMessage = "ItemType is required")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "ItemType must be between 1 and 100 characters")]
        public required string ItemType { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; }

        [StringLength(50, ErrorMessage = "Status cannot exceed 50 characters")]
        public string? Status { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }

        /// <summary>
        /// Optional list of reservations to create along with the booking.
        /// </summary>
        public List<CreateReservationRequest>? Reservations { get; set; }
    }
}

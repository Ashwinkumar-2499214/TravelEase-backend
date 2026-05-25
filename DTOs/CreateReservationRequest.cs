using System.ComponentModel.DataAnnotations;

namespace TravelEase.DTOs
{
    /// <summary>
    /// Request DTO for creating a new reservation within a booking.
    /// </summary>
    public class CreateReservationRequest
    {
        [Required(ErrorMessage = "Details are required")]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "Details must be between 1 and 500 characters")]
        public required string Details { get; set; }

        [Required(ErrorMessage = "StartDate is required")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "EndDate is required")]
        public DateTime EndDate { get; set; }

        [StringLength(50, ErrorMessage = "Status cannot exceed 50 characters")]
        public string? Status { get; set; }
    }
}

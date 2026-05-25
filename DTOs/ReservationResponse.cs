namespace TravelEase.DTOs
{
    /// <summary>
    /// Response DTO for reservation operations.
    /// Contains complete reservation details linked to a booking.
    /// </summary>
    public class ReservationResponse
    {
        public int ReservationID { get; set; }
        public int BookingID { get; set; }
        public string? Details { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

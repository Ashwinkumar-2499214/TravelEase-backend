namespace TravelEase.DTOs
{
    /// <summary>
    /// Response DTO for booking operations.
    /// Contains complete booking details along with associated reservations.
    /// Used for successful creation, retrieval, and modification responses.
    /// </summary>
    public class BookingResponse
    {
        public int BookingID { get; set; }
        public int CustomerID { get; set; }
        public int PartnerID { get; set; }
        public string? ItemType { get; set; }
        public DateTime Date { get; set; }
        public string? Status { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// List of reservations associated with this booking.
        /// </summary>
        public List<ReservationResponse>? Reservations { get; set; }
    }
}

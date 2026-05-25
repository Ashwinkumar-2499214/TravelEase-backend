using TravelEase.DTOs;

namespace TravelEase.Service.Interface
{
    /// <summary>
    /// Service interface for booking management.
    /// Handles creation, retrieval, and search workflows.
    /// </summary>
    public interface IBookingService
    {
        /// <summary>
        /// Retrieves all bookings.
        /// </summary>
        IEnumerable<BookingResponse> GetAllBookings();

        /// <summary>
        /// Retrieves a specific booking by ID with associated reservations.
        /// </summary>
        BookingResponse? GetBooking(int bookingId);

        /// <summary>
        /// Creates a new booking with optional reservations.
        /// Initiates checkout workflow.
        /// </summary>
        BookingResponse CreateBooking(CreateBookingRequest request);

        /// <summary>
        /// Searches bookings by customer ID (booking history).
        /// </summary>
        IEnumerable<BookingResponse> SearchBookingsByCustomer(int customerId);

        /// <summary>
        /// Searches bookings by status.
        /// </summary>
        IEnumerable<BookingResponse> SearchBookingsByStatus(string status);

        /// <summary>
        /// Modifies an existing booking.
        /// </summary>
        BookingResponse? ModifyBooking(int bookingId, CreateBookingRequest request);

        /// <summary>
        /// Cancels a booking.
        /// </summary>
        bool CancelBooking(int bookingId);
    }
}

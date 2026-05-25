namespace TravelEase.Repository.Interface
{
    public interface IBookingRepository
    {
        int CreateBooking(TravelEase.Model.BookingModel booking);
        TravelEase.Model.BookingModel? GetBooking(int bookingId);
        IEnumerable<TravelEase.Model.BookingModel> GetAllBookings();
    }
}

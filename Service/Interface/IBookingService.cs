using TravelEase.ViewModel;
namespace TravelEase.Service.Interface

{
    public interface IBookingService
    {
        IEnumerable<BookingViewModel>  GetAllBookings();
        BookingViewModel GetBooking(int bookingId);

    }
}

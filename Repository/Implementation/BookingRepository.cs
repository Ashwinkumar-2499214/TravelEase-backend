using TravelEase.Repository.Interface;

namespace TravelEase.Repository.Implementation
{
    public class BookingRepository: IBookingRepository
    {
        private static readonly List<TravelEase.Model.BookingModel> _bookings = new();
        private static int _nextId = 1;

        public int CreateBooking(TravelEase.Model.BookingModel booking)
        {
            booking.BookingID = System.Threading.Interlocked.Increment(ref _nextId);
            _bookings.Add(booking);
            return booking.BookingID;
        }

        public TravelEase.Model.BookingModel? GetBooking(int bookingId)
        {
            return _bookings.FirstOrDefault(b => b.BookingID == bookingId);
        }

        public IEnumerable<TravelEase.Model.BookingModel> GetAllBookings()
        {
            return _bookings;
        }
    }
}

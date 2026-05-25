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

        public void UpdateBooking(TravelEase.Model.BookingModel booking)
        {
            var existing = _bookings.FirstOrDefault(b => b.BookingID == booking.BookingID);
            if (existing != null)
            {
                var index = _bookings.IndexOf(existing);
                _bookings[index] = booking;
            }
        }

        public bool DeleteBooking(int bookingId)
        {
            var booking = _bookings.FirstOrDefault(b => b.BookingID == bookingId);
            if (booking != null)
            {
                _bookings.Remove(booking);
                return true;
            }
            return false;
        }
    }
}

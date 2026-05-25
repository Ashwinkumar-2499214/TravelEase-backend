using TravelEase.Repository.Interface;

namespace TravelEase.Repository.Implementation
{
    public class ReservationRepository : IReservationRepository
    {
        private static readonly List<TravelEase.Model.ReservationModel> _reservations = new();
        private static int _nextId = 1;

        public int CreateReservation(TravelEase.Model.ReservationModel reservation)
        {
            reservation.ReservationID = System.Threading.Interlocked.Increment(ref _nextId);
            _reservations.Add(reservation);
            return reservation.ReservationID;
        }

        public IEnumerable<TravelEase.Model.ReservationModel> GetReservationsByBooking(int bookingId)
        {
            return _reservations.Where(r => r.BookingID == bookingId);
        }

        public void UpdateReservation(TravelEase.Model.ReservationModel reservation)
        {
            var existing = _reservations.FirstOrDefault(r => r.ReservationID == reservation.ReservationID);
            if (existing != null)
            {
                var index = _reservations.IndexOf(existing);
                _reservations[index] = reservation;
            }
        }
    }
}
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
	}
}

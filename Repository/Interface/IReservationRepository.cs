namespace TravelEase.Repository.Interface
{
    public interface IReservationRepository
    {
        int CreateReservation(TravelEase.Model.ReservationModel reservation);
        IEnumerable<TravelEase.Model.ReservationModel> GetReservationsByBooking(int bookingId);
        void UpdateReservation(TravelEase.Model.ReservationModel reservation);
    }
}
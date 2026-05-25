using TravelEase.Service.Interface;
using TravelEase.DTOs;
using TravelEase.Model;

namespace TravelEase.Service.Implementation
{
    public class BookingService : IBookingService
    {
        private readonly TravelEase.Repository.Interface.IBookingRepository _bookingRepository;
        private readonly TravelEase.Repository.Interface.IReservationRepository _reservationRepository;

        public BookingService(TravelEase.Repository.Interface.IBookingRepository bookingRepository,
            TravelEase.Repository.Interface.IReservationRepository reservationRepository)
        {
            _bookingRepository = bookingRepository;
            _reservationRepository = reservationRepository;
        }

        public IEnumerable<BookingResponse> GetAllBookings()
        {
            var bookings = _bookingRepository.GetAllBookings();
            return bookings.Select(b => MapToResponse(b));
        }

        public BookingResponse? GetBooking(int bookingId)
        {
            var b = _bookingRepository.GetBooking(bookingId);
            if (b == null) return null;
            return MapToResponse(b);
        }

        public BookingResponse CreateBooking(CreateBookingRequest request)
        {
            var model = new BookingModel
            {
                CustomerID = request.CustomerID,
                PartnerID = request.PartnerID,
                ItemType = request.ItemType,
                Date = request.Date == default ? DateTime.UtcNow : request.Date,
                Status = request.Status ?? "Pending",
                Amount = request.Amount,
                CreatedAt = DateTime.UtcNow
            };

            var bookingId = _bookingRepository.CreateBooking(model);

            var reservationsResponse = new List<ReservationResponse>();
            if (request.Reservations != null && request.Reservations.Count > 0)
            {
                foreach (var r in request.Reservations)
                {
                    var resModel = new ReservationModel
                    {
                        BookingID = bookingId,
                        Details = r.Details,
                        StartDate = r.StartDate,
                        EndDate = r.EndDate,
                        Status = r.Status ?? "Active",
                        CreatedAt = DateTime.UtcNow
                    };
                    var resId = _reservationRepository.CreateReservation(resModel);
                    reservationsResponse.Add(new ReservationResponse
                    {
                        ReservationID = resId,
                        BookingID = bookingId,
                        Details = resModel.Details,
                        StartDate = resModel.StartDate,
                        EndDate = resModel.EndDate,
                        Status = resModel.Status,
                        CreatedAt = resModel.CreatedAt
                    });
                }
            }

            var response = new BookingResponse
            {
                BookingID = bookingId,
                CustomerID = model.CustomerID,
                PartnerID = model.PartnerID,
                ItemType = model.ItemType,
                Date = model.Date,
                Status = model.Status,
                Amount = model.Amount,
                CreatedAt = model.CreatedAt,
                Reservations = reservationsResponse
            };

            return response;
        }

        public IEnumerable<BookingResponse> SearchBookingsByCustomer(int customerId)
        {
            var bookings = _bookingRepository.GetAllBookings()
                .Where(b => b.CustomerID == customerId);
            return bookings.Select(b => MapToResponse(b));
        }

        public IEnumerable<BookingResponse> SearchBookingsByStatus(string status)
        {
            var bookings = _bookingRepository.GetAllBookings()
                .Where(b => b.Status != null && b.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
            return bookings.Select(b => MapToResponse(b));
        }

        public BookingResponse? ModifyBooking(int bookingId, CreateBookingRequest request)
        {
            var booking = _bookingRepository.GetBooking(bookingId);
            if (booking == null) return null;

            booking.Status = request.Status ?? booking.Status;
            booking.Amount = request.Amount;
            booking.UpdatedAt = DateTime.UtcNow;

            _bookingRepository.UpdateBooking(booking);

            return MapToResponse(booking);
        }

        public bool CancelBooking(int bookingId)
        {
            var booking = _bookingRepository.GetBooking(bookingId);
            if (booking == null) return false;

            booking.Status = "Cancelled";
            booking.UpdatedAt = DateTime.UtcNow;

            var reservations = _reservationRepository.GetReservationsByBooking(bookingId).ToList();
            foreach (var res in reservations)
            {
                res.Status = "Cancelled";
                res.UpdatedAt = DateTime.UtcNow;
                _reservationRepository.UpdateReservation(res);
            }

            _bookingRepository.UpdateBooking(booking);
            return true;
        }

        private BookingResponse MapToResponse(BookingModel b)
        {
            var reservations = _reservationRepository.GetReservationsByBooking(b.BookingID)
                .Select(r => new ReservationResponse
                {
                    ReservationID = r.ReservationID,
                    BookingID = r.BookingID,
                    Details = r.Details,
                    StartDate = r.StartDate,
                    EndDate = r.EndDate,
                    Status = r.Status,
                    CreatedAt = r.CreatedAt,
                    UpdatedAt = r.UpdatedAt
                });

            return new BookingResponse
            {
                BookingID = b.BookingID,
                CustomerID = b.CustomerID,
                PartnerID = b.PartnerID,
                ItemType = b.ItemType,
                Date = b.Date,
                Status = b.Status,
                Amount = b.Amount,
                CreatedAt = b.CreatedAt,
                UpdatedAt = b.UpdatedAt,
                Reservations = reservations.ToList()
            };
        }
    }
}

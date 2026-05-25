using TravelEase.Service.Interface;

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

        public IEnumerable<TravelEase.ViewModel.BookingViewModel> GetAllBookings()
        {
            var bookings = _bookingRepository.GetAllBookings();
            return bookings.Select(b => MapToViewModel(b));
        }

        public TravelEase.ViewModel.BookingViewModel GetBooking(int bookingId)
        {
            var b = _bookingRepository.GetBooking(bookingId);
            if (b == null) return new TravelEase.ViewModel.BookingViewModel();
            return MapToViewModel(b);
        }

        public TravelEase.ViewModel.BookingViewModel CreateBooking(TravelEase.ViewModel.CreateBookingRequest request)
        {
            var model = new TravelEase.Model.BookingModel
            {
                CustomerID = request.CustomerID,
                PartnerID = request.PartnerID,
                ItemType = request.ItemType,
                Date = request.Date == default ? DateTime.UtcNow : request.Date,
                Status = request.Status ?? "Pending",
                Amount = request.Amount
            };

            var bookingId = _bookingRepository.CreateBooking(model);

            var reservationsVm = new List<TravelEase.ViewModel.ReservationViewModel>();
            if (request.Reservations != null)
            {
                foreach (var r in request.Reservations)
                {
                    var resModel = new TravelEase.Model.ReservationModel
                    {
                        BookingID = bookingId,
                        Details = r.Details,
                        StartDate = r.StartDate,
                        EndDate = r.EndDate,
                        Status = r.Status ?? "Active"
                    };
                    var resId = _reservationRepository.CreateReservation(resModel);
                    reservationsVm.Add(new TravelEase.ViewModel.ReservationViewModel
                    {
                        ReservationID = resId,
                        BookingID = bookingId,
                        Details = resModel.Details,
                        StartDate = resModel.StartDate,
                        EndDate = resModel.EndDate,
                        Status = resModel.Status
                    });
                }
            }

            var vm = new TravelEase.ViewModel.BookingViewModel
            {
                BookingID = bookingId,
                CustomerID = model.CustomerID,
                PartnerID = model.PartnerID,
                ItemType = model.ItemType,
                Date = model.Date,
                Status = model.Status,
                Amount = model.Amount,
                Reservations = reservationsVm
            };

            return vm;
        }

        private TravelEase.ViewModel.BookingViewModel MapToViewModel(TravelEase.Model.BookingModel b)
        {
            var reservations = _reservationRepository.GetReservationsByBooking(b.BookingID)
                .Select(r => new TravelEase.ViewModel.ReservationViewModel
                {
                    ReservationID = r.ReservationID,
                    BookingID = r.BookingID,
                    Details = r.Details,
                    StartDate = r.StartDate,
                    EndDate = r.EndDate,
                    Status = r.Status
                });

            return new TravelEase.ViewModel.BookingViewModel
            {
                BookingID = b.BookingID,
                CustomerID = b.CustomerID,
                PartnerID = b.PartnerID,
                ItemType = b.ItemType,
                Date = b.Date,
                Status = b.Status,
                Amount = b.Amount,
                Reservations = reservations
            };
        }
    }
}

using System.Collections.Generic;

namespace TravelEase.ViewModel
{
    public class CreateBookingRequest
    {
        public int CustomerID { get; set; }
        public int PartnerID { get; set; }
        public string? ItemType { get; set; }
        public DateTime Date { get; set; }
        public string? Status { get; set; }
        public decimal Amount { get; set; }

        public IEnumerable<CreateReservationRequest>? Reservations { get; set; }
    }

    public class CreateReservationRequest
    {
        public string? Details { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Status { get; set; }
    }
}

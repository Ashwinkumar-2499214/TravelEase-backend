namespace TravelEase.ViewModel
{
    public class BookingViewModel
    {
        public int BookingID { get; set; }
        public int CustomerID { get; set; }
        public int PartnerID { get; set; }
        public string? ItemType { get; set; }
        public DateTime Date { get; set; }
        public string? Status { get; set; }
        public decimal Amount { get; set; }
        public IEnumerable<ReservationViewModel>? Reservations { get; set; }
    }
}

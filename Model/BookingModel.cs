namespace TravelEase.Model
{
    public class BookingModel
    {
        public int BookingID { get; set; }
        public int CustomerID { get; set; }
        public int PartnerID { get; set; }
        public string? ItemType { get; set; }
        public DateTime Date { get; set; }
        public string? Status { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}

namespace TravelEase.Model
{
	public class ReservationModel
	{
		public int ReservationID { get; set; }
		public int BookingID { get; set; }
		public string? Details { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public string? Status { get; set; }
	}
}

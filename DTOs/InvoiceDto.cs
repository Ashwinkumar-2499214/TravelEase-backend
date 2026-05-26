public class InvoiceDto
{
    public Guid InvoiceId { get; set; }
    public Guid BookingId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string Status { get; set; }
}
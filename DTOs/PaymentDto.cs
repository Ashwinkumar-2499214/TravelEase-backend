public class PaymentDto
{
    public Guid PaymentId { get; set; }
    public Guid InvoiceId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public required string Method { get; set; }
    public required string Status { get; set; }
}
public class Payment
{
    public Guid PaymentId { get; set; }
    public Guid InvoiceId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public PaymentMethod Method { get; set; }
    public PaymentStatus Status { get; set; }
}
namespace TravelEaseBackend.Dto
{
    public class PaymentSearchDto
    {
        public string? Status { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? Method { get; set; }
    }

    public class InvoiceSearchDto
    {
        public string? Status { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}

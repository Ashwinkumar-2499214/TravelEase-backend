public interface IPaymentService
{
    Task<IEnumerable<PaymentDto>> GetAllPaymentsAsync();
    Task<IEnumerable<PaymentDto>> GetPaymentsByInvoiceIdAsync(Guid invoiceId);
    Task<PaymentDto?> GetPaymentByIdAsync(Guid paymentId);
    Task<PaymentDto> CreatePaymentAsync(Guid invoiceId, CreatePaymentRequest request);
    Task UpdatePaymentStatusAsync(Guid paymentId, string status);
    Task RefundPaymentAsync(Guid paymentId);
    Task ApplyAdjustmentAsync(Guid invoiceId, decimal adjustmentAmount);
}
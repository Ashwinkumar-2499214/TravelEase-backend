public interface IPaymentRepository
{
    Task<IEnumerable<Payment>> GetAllAsync();
    Task<IEnumerable<Payment>> GetByInvoiceIdAsync(Guid invoiceId);
    Task<Payment?> GetByIdAsync(Guid paymentId);
    Task AddAsync(Payment payment);
    Task UpdateAsync(Payment payment);
}
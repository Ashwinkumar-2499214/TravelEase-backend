using Microsoft.EntityFrameworkCore;

public class PaymentRepository : IPaymentRepository
{
    private readonly AppDbContext _context;
    public PaymentRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<Payment>> GetAllAsync() =>
        await _context.Payments.ToListAsync();

    public async Task<IEnumerable<Payment>> GetByInvoiceIdAsync(Guid invoiceId) =>
        await _context.Payments.Where(p => p.InvoiceId == invoiceId).ToListAsync();

    public async Task<Payment?> GetByIdAsync(Guid paymentId) =>
        await _context.Payments.FindAsync(paymentId);

    public async Task AddAsync(Payment payment)
    {
        await _context.Payments.AddAsync(payment);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Payment payment)
    {
        _context.Payments.Update(payment);
        await _context.SaveChangesAsync();
    }
}

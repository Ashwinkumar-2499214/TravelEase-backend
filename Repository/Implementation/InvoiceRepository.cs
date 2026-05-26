using Microsoft.EntityFrameworkCore;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly AppDbContext _context;
    public InvoiceRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<Invoice>> GetAllAsync() => await _context.Invoices.ToListAsync();

    public async Task<Invoice?> GetByIdAsync(Guid invoiceId) => await _context.Invoices.FindAsync(invoiceId);

    public async Task AddAsync(Invoice invoice)
    {
        await _context.Invoices.AddAsync(invoice);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Invoice invoice)
    {
        _context.Invoices.Update(invoice);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid invoiceId)
    {
        var invoice = await GetByIdAsync(invoiceId);
        if (invoice != null)
        {
            _context.Invoices.Remove(invoice);
            await _context.SaveChangesAsync();
        }
    }
}

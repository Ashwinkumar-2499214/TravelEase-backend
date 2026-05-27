using TravelEaseBackend.Dto;
using TravelEaseBackend.Models;
using TravelEaseBackend.Repository.Interface;
using System.Linq;

public class InvoiceService : IInvoiceService
{
    private readonly IInvoiceRepository _invoiceRepo;
    public InvoiceService(IInvoiceRepository invoiceRepo) => _invoiceRepo = invoiceRepo;

    public async Task<IEnumerable<InvoiceDto>> GetAllInvoicesAsync(InvoiceSearchDto search)
    {
        var invoices = (await _invoiceRepo.GetAllAsync()).AsEnumerable();

        if (!string.IsNullOrWhiteSpace(search?.Status))
        {
            invoices = invoices.Where(i => i.Status.ToString().Equals(search.Status, StringComparison.OrdinalIgnoreCase));
        }

        if (search?.FromDate.HasValue == true)
        {
            invoices = invoices.Where(i => i.Date >= search.FromDate.Value);
        }

        if (search?.ToDate.HasValue == true)
        {
            invoices = invoices.Where(i => i.Date <= search.ToDate.Value);
        }

        return invoices.Select(i => new InvoiceDto
        {
            InvoiceId = i.InvoiceId,
            BookingId = i.BookingId,
            Amount = i.Amount,
            Date = i.Date,
            Status = i.Status.ToString()
        });
    }

    public async Task<InvoiceDto?> GetInvoiceByIdAsync(Guid invoiceId)
    {
        var invoice = await _invoiceRepo.GetByIdAsync(invoiceId);
        return invoice == null ? null : new InvoiceDto
        {
            InvoiceId = invoice.InvoiceId,
            BookingId = invoice.BookingId,
            Amount = invoice.Amount,
            Date = invoice.Date,
            Status = invoice.Status.ToString()
        };
    }

    public async Task<InvoiceDto> CreateInvoiceAsync(CreateInvoiceRequest request)
    {
        var invoice = new Invoice
        {
            InvoiceId = Guid.NewGuid(),
            BookingId = request.BookingId,
            Amount = request.Amount,
            Date = DateTime.UtcNow,
            Status = InvoiceStatus.Unpaid
        };
        await _invoiceRepo.AddAsync(invoice);
#pragma warning disable CS8603 // Possible null reference return.
        return await GetInvoiceByIdAsync(invoice.InvoiceId)!;
#pragma warning restore CS8603 // Possible null reference return.
    }

    public async Task UpdateInvoiceAsync(Guid invoiceId, InvoiceDto dto)
    {
        var invoice = await _invoiceRepo.GetByIdAsync(invoiceId);
        if (invoice == null) throw new Exception("Invoice not found");
        invoice.Amount = dto.Amount;
        invoice.Status = Enum.Parse<InvoiceStatus>(dto.Status);
        await _invoiceRepo.UpdateAsync(invoice);
    }

    public async Task DeleteInvoiceAsync(Guid invoiceId) => await _invoiceRepo.DeleteAsync(invoiceId);

    public async Task UpdateInvoiceStatusAsync(Guid invoiceId, string status)
    {
        var invoice = await _invoiceRepo.GetByIdAsync(invoiceId);
        if (invoice == null) throw new Exception("Invoice not found");
        invoice.Status = Enum.Parse<InvoiceStatus>(status);
        await _invoiceRepo.UpdateAsync(invoice);
    }
}
using TravelEaseBackend.Dto;

public interface IInvoiceService
{
    Task<IEnumerable<InvoiceDto>> GetAllInvoicesAsync(InvoiceSearchDto search);
    Task<InvoiceDto?> GetInvoiceByIdAsync(Guid invoiceId);
    Task<InvoiceDto> CreateInvoiceAsync(CreateInvoiceRequest request);
    Task UpdateInvoiceAsync(Guid invoiceId, InvoiceDto invoiceDto);
    Task DeleteInvoiceAsync(Guid invoiceId);
    Task UpdateInvoiceStatusAsync(Guid invoiceId, string status);
}
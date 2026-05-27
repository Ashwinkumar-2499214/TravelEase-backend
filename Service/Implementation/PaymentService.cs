using TravelEaseBackend.Dto;
using TravelEaseBackend.Models;
using TravelEaseBackend.Repository.Interface;
using TravelEaseBackend.Services.Interface;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepo;
    private readonly IInvoiceRepository _invoiceRepo;

    public PaymentService(IPaymentRepository paymentRepo, IInvoiceRepository invoiceRepo)
    {
        _paymentRepo = paymentRepo;
        _invoiceRepo = invoiceRepo;
    }

    public async Task<IEnumerable<PaymentDto>> GetAllPaymentsAsync(PaymentSearchDto search)
    {
        var payments = (await _paymentRepo.GetAllAsync()).AsEnumerable();

        // Filter by status if provided
        if (!string.IsNullOrWhiteSpace(search.Status))
        {
            payments = payments.Where(p => p.Status.ToString().Equals(search.Status, StringComparison.OrdinalIgnoreCase));
        }

        // Filter by date range if provided
        if (search.FromDate.HasValue)
        {
            payments = payments.Where(p => p.Date >= search.FromDate.Value);
        }
        if (search.ToDate.HasValue)
        {
            payments = payments.Where(p => p.Date <= search.ToDate.Value);
        }

        // Filter by method if provided
        if (!string.IsNullOrWhiteSpace(search.Method))
        {
            payments = payments.Where(p => p.Method.ToString().Equals(search.Method, StringComparison.OrdinalIgnoreCase));
        }

        return payments.Select(p => new PaymentDto
        {
            PaymentId = p.PaymentId,
            InvoiceId = p.InvoiceId,
            Amount = p.Amount,
            Date = p.Date,
            Method = p.Method.ToString(),
            Status = p.Status.ToString()
        });
    }

    public async Task<IEnumerable<PaymentDto>> GetPaymentsByInvoiceIdAsync(Guid invoiceId) =>
        (await _paymentRepo.GetByInvoiceIdAsync(invoiceId)).Select(p => new PaymentDto
        {
            PaymentId = p.PaymentId,
            InvoiceId = p.InvoiceId,
            Amount = p.Amount,
            Date = p.Date,
            Method = p.Method.ToString(),
            Status = p.Status.ToString()
        });

    public async Task<PaymentDto?> GetPaymentByIdAsync(Guid paymentId)
    {
        var payment = await _paymentRepo.GetByIdAsync(paymentId);
        return payment == null ? null : new PaymentDto
        {
            PaymentId = payment.PaymentId,
            InvoiceId = payment.InvoiceId,
            Amount = payment.Amount,
            Date = payment.Date,
            Method = payment.Method.ToString(),
            Status = payment.Status.ToString()
        };
    }

    public async Task<PaymentDto> CreatePaymentAsync(Guid invoiceId, CreatePaymentRequest request)
    {
        var invoice = await _invoiceRepo.GetByIdAsync(invoiceId);
        if (invoice == null) throw new Exception("Invoice not found");

        var payment = new Payment
        {
            PaymentId = Guid.NewGuid(),
            InvoiceId = invoiceId,
            Amount = request.Amount,
            Date = DateTime.UtcNow,
            Method = Enum.Parse<PaymentMethod>(request.Method),
            Status = PaymentStatus.Pending
        };

        await _paymentRepo.AddAsync(payment);

        // Update invoice status if fully paid
        if (request.Amount >= invoice.Amount)
        {
            invoice.Status = InvoiceStatus.Paid;
            await _invoiceRepo.UpdateAsync(invoice);
        }

#pragma warning disable CS8603 // Possible null reference return.
        return await GetPaymentByIdAsync(payment.PaymentId)!;
#pragma warning restore CS8603 // Possible null reference return.
    }

    public async Task UpdatePaymentStatusAsync(Guid paymentId, string status)
    {
        var payment = await _paymentRepo.GetByIdAsync(paymentId);
        if (payment == null) throw new Exception("Payment not found");
        payment.Status = Enum.Parse<PaymentStatus>(status);
        await _paymentRepo.UpdateAsync(payment);
    }

    public async Task RefundPaymentAsync(Guid paymentId)
    {
        var payment = await _paymentRepo.GetByIdAsync(paymentId);
        if (payment == null) throw new Exception("Payment not found");

        payment.Status = PaymentStatus.Refunded;
        await _paymentRepo.UpdateAsync(payment);

        var invoice = await _invoiceRepo.GetByIdAsync(payment.InvoiceId);
        if (invoice != null)
        {
            invoice.Status = InvoiceStatus.Unpaid;
            await _invoiceRepo.UpdateAsync(invoice);
        }
    }

    public async Task ApplyAdjustmentAsync(Guid invoiceId, decimal adjustmentAmount)
    {
        var invoice = await _invoiceRepo.GetByIdAsync(invoiceId);
        if (invoice == null) throw new Exception("Invoice not found");

        invoice.Amount += adjustmentAmount;
        await _invoiceRepo.UpdateAsync(invoice);
    }

    public async Task<IEnumerable<PaymentDto>> SearchPaymentsAsync(PaymentSearchDto searchCriteria)
    {
        var allPayments = await _paymentRepo.GetAllAsync();
        var query = allPayments.AsEnumerable();

        // Filter by status
        if (!string.IsNullOrWhiteSpace(searchCriteria.Status))
        {
            query = query.Where(p => p.Status.ToString().Equals(searchCriteria.Status, StringComparison.OrdinalIgnoreCase));
        }

        // Filter by date range
        if (searchCriteria.FromDate.HasValue)
        {
            query = query.Where(p => p.Date >= searchCriteria.FromDate.Value);
        }

        if (searchCriteria.ToDate.HasValue)
        {
            query = query.Where(p => p.Date <= searchCriteria.ToDate.Value);
        }

        // Filter by method
        if (!string.IsNullOrWhiteSpace(searchCriteria.Method))
        {
            query = query.Where(p => p.Method.ToString().Equals(searchCriteria.Method, StringComparison.OrdinalIgnoreCase));
        }

        return query.Select(p => new PaymentDto
        {
            PaymentId = p.PaymentId,
            InvoiceId = p.InvoiceId,
            Amount = p.Amount,
            Date = p.Date,
            Method = p.Method.ToString(),
            Status = p.Status.ToString()
        });
    }
}
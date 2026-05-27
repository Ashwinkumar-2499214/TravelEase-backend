using Microsoft.AspNetCore.Mvc;
using TravelEaseBackend.Dto;

[ApiController]
[Route("api/v1/payments")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    public PaymentController(IPaymentService paymentService) => _paymentService = paymentService;

    //Get all payments
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PaymentSearchDto search)
    {
        try
        {
            var payments = await _paymentService.GetAllPaymentsAsync(search);
            return Ok(payments);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
        }
    }

    // Get Payment by ID
    [HttpGet("{paymentId}")]
    public async Task<IActionResult> GetById(Guid paymentId)
    {
        try
        {
            var payment = await _paymentService.GetPaymentByIdAsync(paymentId);
            if (payment == null) return NotFound();
            return Ok(payment);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
        }
    }

    //Get payments by invoice
    [HttpGet("/api/v1/invoices/{invoiceId}/payments")]
    public async Task<IActionResult> GetByInvoice(Guid invoiceId)
    {
        try
        {
            var payments = await _paymentService.GetPaymentsByInvoiceIdAsync(invoiceId);
            return Ok(payments);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    //Create payment for invoice
    [HttpPost("/api/v1/invoices/{invoiceId}/payments")]
    public async Task<IActionResult> Create(Guid invoiceId, [FromBody] CreatePaymentRequest request)
    {
        try
        {
            var payment = await _paymentService.CreatePaymentAsync(invoiceId, request);
            return CreatedAtAction(nameof(GetById), new { paymentId = payment.PaymentId }, payment);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    //Update payment status
    [HttpPatch("{paymentId}/status")]
    public async Task<IActionResult> UpdateStatus(Guid paymentId, [FromBody] string status)
    {
        try
        {
            await _paymentService.UpdatePaymentStatusAsync(paymentId, status);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    //Refund a payment
    [HttpPost("{paymentId}/refund")]
    public async Task<IActionResult> Refund(Guid paymentId)
    {
        try
        {
            await _paymentService.RefundPaymentAsync(paymentId);
            return Ok(new { message = "Refund processed successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
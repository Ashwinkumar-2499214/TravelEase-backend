using Microsoft.AspNetCore.Mvc;
using TravelEaseBackend.Dto;

[ApiController]
[Route("api/v1/invoices")]
public class InvoiceController : ControllerBase
{
    private readonly IInvoiceService _invoiceService;
    public InvoiceController(IInvoiceService invoiceService) => _invoiceService = invoiceService;
//Get all invoices
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] InvoiceSearchDto search)
    {
        try
        {
            var invoices = await _invoiceService.GetAllInvoicesAsync(search);
            return Ok(invoices);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
        }
    }
//Create invoice
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateInvoiceRequest request)
    {
        try
        {
            var invoice = await _invoiceService.CreateInvoiceAsync(request);
            return CreatedAtAction(nameof(GetById), new { invoiceId = invoice.InvoiceId }, invoice);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
//Get invoice by ID
    [HttpGet("{invoiceId}")]
    public async Task<IActionResult> GetById(Guid invoiceId)
    {
        try
        {
            var invoice = await _invoiceService.GetInvoiceByIdAsync(invoiceId);
            if (invoice == null) return NotFound();
            return Ok(invoice);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
        }
    }


//Delete invoice
    [HttpDelete("{invoiceId}")]
    public async Task<IActionResult> Delete(Guid invoiceId)
    {
        try
        {
            await _invoiceService.DeleteInvoiceAsync(invoiceId);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
//Update invoice status
    [HttpPatch("{invoiceId}/status")]
    public async Task<IActionResult> UpdateStatus(Guid invoiceId, [FromBody] string status)
    {
        try
        {
            await _invoiceService.UpdateInvoiceStatusAsync(invoiceId, status);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}